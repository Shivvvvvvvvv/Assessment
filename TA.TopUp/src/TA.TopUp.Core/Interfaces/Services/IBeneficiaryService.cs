using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Shared.DTOs.Request;
using TA.TopUp.Shared.DTOs.Response;

namespace TA.TopUp.Core.Interfaces.Services
{
    public interface IBeneficiaryService
    {
        Task<BeneficiaryResponse> SaveBeneficiary(int userId, SaveBeneficiaryRequest saveBeneficiaryRequest);
        Task<BeneficiaryResponse> DeleteBeneficiary(int userId, int Id);
        Task<BeneficiaryResponse> UpdateBeneficiary(int userId, UpdateBeneficiaryRequest saveBeneficiaryRequest);
        Task<IEnumerable<FetchBeneficiaryResponse>> FetchBeneficiary(int userId);
    }
}
