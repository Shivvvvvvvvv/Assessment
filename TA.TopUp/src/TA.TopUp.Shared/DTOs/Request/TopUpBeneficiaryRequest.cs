using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Shared.DTOs.Request
{
    public class TopUpBeneficiaryRequest
    {
        public decimal Amount { get; set; }
        public int BeneficiaryId { get; set; }
        public string Currency { get; set; }
    }
}
