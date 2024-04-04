using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Core.Entities
{
    public partial class UserWalletBalance
    {
        public long Uid { get; set; }

        public long? UserId { get; set; }

        public decimal? Balance { get; set; }

        public long? CurrencyId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public virtual Currency? Currency { get; set; }

        public virtual User? User { get; set; }
    }
}
