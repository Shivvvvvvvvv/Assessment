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
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly ILogger<BeneficiaryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BeneficiariesTopUpValidation _beneficiariesTopUpValidation;
        public BeneficiaryService(ILogger<BeneficiaryService> logger, IUnitOfWork unitOfWork, IOptionsMonitor<BeneficiariesTopUpValidation> beneficiariesTopUpValidation)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _beneficiariesTopUpValidation = beneficiariesTopUpValidation.CurrentValue;
        }

        public async Task<BeneficiaryResponse> DeleteBeneficiary(int userId, int Id)
        {
            BeneficiaryResponse response = new BeneficiaryResponse();
            try
            {
                var beneficiary = (await _unitOfWork.BeneficiaryRepository.GetById(Convert.ToInt64(Id)));
                if (beneficiary?.Uid > 0)
                {
                    //Soft Delete we are making the beneficiary inactive and updating the last updated date and by.
                    beneficiary.IsActive = false;
                    beneficiary.LastUpdatedAt = DateTime.Now;
                    beneficiary.LastUpdatedBy = userId.ToString();
                    _unitOfWork.BeneficiaryRepository.Update(beneficiary);
                    response.IsSuccess = await _unitOfWork.SaveEntitiesAsync();
                    response.Message = "Beneficiary deleted";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Beneficiary not found";
                }
                
            }
            catch (Exception ex)
            {
                string message = "An error occured while deleting an beneficiary";
                _logger.LogError(ex, message);
                response.IsSuccess = false;
                response.Message = message;
            }
            return response;
        }

        public async Task<IEnumerable<FetchBeneficiaryResponse>> FetchBeneficiary(int userId)
        {
            var beneficiaries = (await _unitOfWork.BeneficiaryRepository.GetAsync(x => x.UserId == userId && x.IsActive == true))
                                .Select(s=> new FetchBeneficiaryResponse
                                {
                                    UId = s.Uid,
                                    NickName = s.NickName,
                                    MobileNumber = s.MobileNumber,
                                    CountryCode = s.CountryCode
                                });
            return beneficiaries;
        }

        public async Task<BeneficiaryResponse> SaveBeneficiary(int userId, SaveBeneficiaryRequest saveBeneficiaryRequest)
        {
            BeneficiaryResponse response = new BeneficiaryResponse();
            try
            {
                var user = (await _unitOfWork.BeneficiaryRepository.GetAsync(x => x.User.Uid == userId)).Count();
                if (user > 0) 
                {
                    _logger.LogInformation("Creating Beneficiary");
                    var beneficiaryCount = (await _unitOfWork.BeneficiaryRepository.GetAsync(x => x.UserId == userId && x.IsActive == true)).Count();
                    //Checking maximum beneficiary per user
                    if (beneficiaryCount > _beneficiariesTopUpValidation.MaxBeneficiaryPerUser)
                    {
                        string message = "Exceeded maximum beneficiary per user";
                        response.IsSuccess = false;
                        response.Message = message;
                    }
                    else
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary.UserId = userId;
                        beneficiary.IsActive = true;
                        beneficiary.NickName = saveBeneficiaryRequest.NickName;
                        beneficiary.MobileNumber = saveBeneficiaryRequest.MobileNumber;
                        beneficiary.CountryCode = saveBeneficiaryRequest.CountryCode;
                        beneficiary.CreatedAt = DateTime.Now;
                        beneficiary.CreatedBy = userId.ToString();
                        beneficiary.LastUpdatedBy = userId.ToString();
                        beneficiary.LastUpdatedAt = DateTime.Now;

                        _unitOfWork.BeneficiaryRepository.Insert(beneficiary);
                        response.IsSuccess = await _unitOfWork.SaveEntitiesAsync();
                        response.Message = "Saved Successfully";
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid user";

                }
                
            }
            catch (Exception ex)
            {
                string message = "An error occured while saving an beneficiary";
                _logger.LogError(ex, message);
                response.IsSuccess = false;
                response.Message = message;
            }
            

            return response;
        }

        public async Task<BeneficiaryResponse> UpdateBeneficiary(int userId, UpdateBeneficiaryRequest req)
        {
            BeneficiaryResponse response = new BeneficiaryResponse();
            try
            {
                var beneficiary = (await _unitOfWork.BeneficiaryRepository.GetById(req.Id));
                if(beneficiary?.Uid >0)
                {
                    beneficiary.NickName = req.NickName;
                    beneficiary.MobileNumber = req.MobileNumber;
                    beneficiary.CountryCode = req.CountryCode;
                    beneficiary.LastUpdatedAt = DateTime.Now;
                    beneficiary.LastUpdatedBy = userId.ToString();
                    _unitOfWork.BeneficiaryRepository.Update(beneficiary);
                    response.IsSuccess = await _unitOfWork.SaveEntitiesAsync();
                    response.Message = "Beneficiary Updated";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Beneficiary not found";
                }
                
            }
            catch (Exception ex)
            {
                string message = "An error occured while updated an beneficiary";
                _logger.LogError(ex, message);
                response.IsSuccess = false;
                response.Message = message;
            }
            return response;
        }
    }
}
