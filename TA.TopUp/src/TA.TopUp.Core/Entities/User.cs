using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Core.Entities
{
    public partial class User
    {
        public long Uid { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool? IsVerified { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

        public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();

        public virtual ICollection<UserWalletBalance> UserWalletBalances { get; set; } = new List<UserWalletBalance>();
    }
}
