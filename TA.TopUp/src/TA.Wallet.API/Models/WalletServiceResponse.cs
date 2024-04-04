namespace TA.Wallet.API.Models
{
    public class WalletServiceResponse
    {

        public long WalletId { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
    }
}
