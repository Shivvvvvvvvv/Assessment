using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Core.Entities
{
    public partial class UserTransaction
    {
        public long Uid { get; set; }

        public long? UserId { get; set; }

        public string? TransactionType { get; set; }

        public decimal? Amount { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public long? BeneficiaryId { get; set; }

        public long? CurrencyId { get; set; }

        public virtual Beneficiary? Beneficiary { get; set; }

        public virtual Currency? Currency { get; set; }

        public virtual User? User { get; set; }
    }
}
