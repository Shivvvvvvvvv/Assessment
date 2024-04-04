using TA.Wallet.API.Models;

namespace TA.Wallet.API.Interfaces
{
    public interface IWalletService
    {
        Task<WalletServiceResponse> GetWallentBalanceByUser(int userId);
    }
}
