using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Shared.DTOs.Response
{
    public class FetchBeneficiaryResponse
    {
        public long UId { get; set; }
        public string? NickName { get; set; }
        public string? MobileNumber { get; set; }
        public string? CountryCode { get; set; }
    }
}
