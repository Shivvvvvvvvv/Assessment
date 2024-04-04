using Microsoft.AspNetCore.Mvc;
using TA.TopUp.ApplicationService;
using TA.TopUp.Core.Interfaces.Services;
using TA.TopUp.Shared.DTOs.Request;

namespace TA.TopUp.API.Controllers
{
    [Route("beneficiary")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ILogger<BeneficiaryController> _log;

        public BeneficiaryController(IBeneficiaryService beneficiaryService, ILogger<BeneficiaryController> log)
        {
            _beneficiaryService = beneficiaryService;
            _log = log;

        }

        [HttpGet("user-beneficiary")]
        public async Task<IResult> FetchBeneficiaries([FromHeader(Name = "UserId")] int userId)
        {
            _log.LogInformation("Fetch Beneficiary");
            var result = await _beneficiaryService.FetchBeneficiary(userId);
            return Results.Ok(result);
        }

        [HttpPost("add-beneficiary")]
        public async Task<IResult> SaveBeneficiary([FromHeader(Name = "UserId")] int userId, SaveBeneficiaryRequest req)
        {
            _log.LogInformation("Add Beneficiary");
            var result = await _beneficiaryService.SaveBeneficiary(userId,req);
            return Results.Ok(result);

        }

        [HttpGet("delete-beneficiary")]
        public async Task<IResult> DeleteBeneficiary([FromHeader(Name = "UserId")] int userId, int beneficiaryId)
        {
            _log.LogInformation("Delete Beneficiary");
            var result = await _beneficiaryService.DeleteBeneficiary(userId, beneficiaryId);
            return Results.Ok(result);

        }

        [HttpPost("update-beneficiary")]
        public async Task<IResult> UpdateBeneficiary([FromHeader(Name = "UserId")] int userId, UpdateBeneficiaryRequest req)
        {
            _log.LogInformation("Delete Beneficiary");
            var result = await _beneficiaryService.UpdateBeneficiary(userId, req);
            return Results.Ok(result);

        }
    }
}
