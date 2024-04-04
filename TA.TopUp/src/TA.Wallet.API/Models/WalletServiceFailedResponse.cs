namespace TA.Wallet.API.Models
{
    public class WalletServiceFailedResponse
    {
        public string? Message { get; set; }
        public string? Code { get; set; }
        public DateTime TimeStamp{ get; set; }
    }
}
