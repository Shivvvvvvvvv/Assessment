using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Core.Entities
{
    public partial class Currency
    {
        public long Uid { get; set; }

        public string? Currency1 { get; set; }

        public virtual ICollection<TopUpOption> TopUpOptions { get; set; } = new List<TopUpOption>();

        public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();

        public virtual ICollection<UserWalletBalance> UserWalletBalances { get; set; } = new List<UserWalletBalance>();
    }
}
