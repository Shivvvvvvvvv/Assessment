using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Shared.DTOs.Request;
using TA.TopUp.Shared.DTOs.Response;

namespace TA.TopUp.Core.Interfaces.Services
{
    public interface ITopUPService
    {
        Task<TopUpResponse> TopUpBeneficiary(int userId, TopUpBeneficiaryRequest request);
        Task<IEnumerable<TopUpOptionResponse>> GetTopUpOptions(int userId);

    }
}
