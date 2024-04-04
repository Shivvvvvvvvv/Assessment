using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Core.Entities;
using TA.TopUp.Core.Interfaces.Services;
using TA.TopUp.Infrastructure.DataAccessAbstractions;
using TA.TopUp.Shared.DTOs.Request;
using TA.TopUp.Shared.DTOs.Response;
using TA.TopUp.Shared.Options;

namespace TA.TopUp.ApplicationService
{
    public class TopUpService : ITopUPService
    {
        private readonly ILogger<TopUpService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BeneficiariesTopUpValidation _beneficiariesTopUpValidation;
        public TopUpService(ILogger<TopUpService> logger, IUnitOfWork unitOfWork, IOptionsMonitor<BeneficiariesTopUpValidation> beneficiariesTopUpValidation) 
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _beneficiariesTopUpValidation = beneficiariesTopUpValidation.CurrentValue;
        }
        public async Task<TopUpResponse> TopUpBeneficiary(int userId, TopUpBeneficiaryRequest request)
        {
            TopUpResponse topUpResponse = new TopUpResponse();
            try
            {
                var beneficiary = (await _unitOfWork.BeneficiaryRepository.GetAsync(x => x.UserId == userId && x.Uid == request.BeneficiaryId, y=> y.User )).FirstOrDefault();
                if (beneficiary != null)
                {
                    //maximum beneficiary amount
                    int maxBeneficiaryAmountPerMonth = beneficiary.User?.IsVerified == true ? _beneficiariesTopUpValidation.MaxTopUpPerVerifiedUserPerBenAmt : _beneficiariesTopUpValidation.MaxTopUpPerUnVerifiedUserPerBenAmt;

                    //Maximum Topup Cap per user
                    int maxTopUpAmountPerMonth = _beneficiariesTopUpValidation.MaxTopUpPerBenPerMonth;//need to be renamed

                    //Getting Startdate and end date of current month
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    //Reading permonth total topup based on beneficiary
                    var topUpTransactionPer = (await _unitOfWork.UserTransactionsRepository.GetAsync(x => x.TransactionType =="Debit" && x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.UserId == userId)).Select(x => new
                                                            {
                                                                x.UserId,
                                                                x.Uid,
                                                                x.Amount,
                                                                x.BeneficiaryId,
                                                                x.CreatedAt
                                                            });
                    //Total transaction per month
                    decimal? totalTransactionPerMonth = topUpTransactionPer.Sum(y => y.Amount);

                    //Total Transaction per month based on beneficiary
                    decimal? totalTransactionPerBeneficiary = topUpTransactionPer.Where(x=>x.BeneficiaryId == beneficiary.Uid).Sum(y => y.Amount);

                    //Validating - Refactor below method
                    if(totalTransactionPerMonth < maxTopUpAmountPerMonth && totalTransactionPerBeneficiary < maxBeneficiaryAmountPerMonth)
                    {
                        //Check topup amount
                        var verifyTopUpOption = (await _unitOfWork.TopUpOptionsRepository.GetAsync(x => x.Amount == request.Amount)).FirstOrDefault();

                        //Refract below methos
                        if(verifyTopUpOption != null)
                        {
                            //user Balance
                            int topUpBalance = 10;
                            long walletId = 1;

                            //balance

                            //Debit from wallet balance
                            UserWalletBalance userWalletBalance = new UserWalletBalance();
                            userWalletBalance.Uid = walletId;
                            userWalletBalance.UserId = userId;
                            userWalletBalance.Balance = topUpBalance - request.Amount;
                            userWalletBalance.CurrencyId = 1;
                            
                            userWalletBalance.LastUpdatedAt = DateTime.UtcNow;
                            userWalletBalance.LastUpdatedBy = userId.ToString();

                            _unitOfWork.UserWalletBalancesRepository.Update(userWalletBalance);

                            //Insert into transaction table
                            UserTransaction userTransaction = new UserTransaction();
                            userTransaction.UserId = userId;
                            userTransaction.BeneficiaryId = request.BeneficiaryId;
                            userTransaction.Amount = request.Amount;
                            userTransaction.TransactionType = "Debit";
                            userTransaction.CurrencyId = 1;
                            userTransaction.CreatedAt = DateTime.UtcNow;
                            userTransaction.CreatedBy = userId.ToString();

                            _unitOfWork.UserTransactionsRepository.Insert(userTransaction);

                            topUpResponse.IsSuccess = await _unitOfWork.SaveEntitiesAsync();
                            topUpResponse.Message = "Topup succeed";

                        }
                        else
                        {
                            topUpResponse.IsSuccess = false;
                            topUpResponse.Message = "Invalid topup amount";
                        }




                    }
                    else
                    {
                        topUpResponse.IsSuccess = false;
                        topUpResponse.Message = "Maximum limit exceed";
                    }
                }
                else
                {
                    topUpResponse.IsSuccess = false;
                    topUpResponse.Message = "Beneficiary not Found";
                }

            }
            catch (Exception ex)
            {
                string message = "TopUp service failed unexpectedly";
                _logger.LogError(ex, message);
                topUpResponse.IsSuccess = false;
                topUpResponse.Message = message;

            }
            return topUpResponse;
        }

        public async Task<IEnumerable<TopUpOptionResponse>> GetTopUpOptions(int userId)
        {
            var topUpOptions = (await _unitOfWork.TopUpOptionsRepository.GetAsync(x=>x.Amount != null))
                               .Select(s => new TopUpOptionResponse
                               {
                                   UId = s.Uid,
                                   Amount = s.Amount,
                                   Currency = s.Currency?.Currency1??"AED"
                               });
            return topUpOptions;
        }
    }
}
