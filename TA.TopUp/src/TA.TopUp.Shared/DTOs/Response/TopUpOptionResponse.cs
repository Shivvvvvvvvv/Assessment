using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Shared.DTOs.Response
{
    public class TopUpOptionResponse
    {
        public long UId { get; set; }
        public int? Amount { get; set; }
        public string? Currency { get; set; }
    }
}
