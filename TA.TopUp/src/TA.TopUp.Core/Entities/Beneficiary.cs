using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Core.Entities
{
    public partial class Beneficiary
    {
        public long Uid { get; set; }

        public string? NickName { get; set; }

        public string? MobileNumber { get; set; }

        public string? CountryCode { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public long? UserId { get; set; }

        public bool? IsActive { get; set; }

        public virtual User? User { get; set; }

        public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
    }
}
