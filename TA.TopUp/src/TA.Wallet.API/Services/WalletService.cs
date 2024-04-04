using TA.TopUp.Core.Interfaces.Repositories;
using TA.TopUp.Infrastructure.DataAccessAbstractions;
using TA.Wallet.API.Interfaces;
using TA.Wallet.API.Models;

namespace TA.Wallet.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService( IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletServiceResponse> GetWallentBalanceByUser(int userId)
        {
            var userBalance = (await _unitOfWork.UserWalletBalancesRepository.GetAsync(x => x.UserId == userId, i=>i.Currency)).Select(y=> new WalletServiceResponse 
            { 
            WalletId = y.Uid,
            Amount = y.Balance,
            Currency = y.Currency?.Currency1 ?? "AED"
            }).FirstOrDefault();

            return userBalance;


        }
    }
}
