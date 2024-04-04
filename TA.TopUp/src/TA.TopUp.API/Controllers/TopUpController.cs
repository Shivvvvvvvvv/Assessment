using Microsoft.AspNetCore.Mvc;
using TA.TopUp.Core.Interfaces.Services;
using TA.TopUp.Shared.DTOs.Request;

namespace TA.TopUp.API.Controllers
{
    [Route("topup")]
    [ApiController]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUPService _topupService;
        private readonly ILogger<TopUpController> _log;

        public TopUpController(ITopUPService topupService, ILogger<TopUpController> log)
        {
            _topupService = topupService;
            _log = log;

        }

        [HttpGet("options")]
        public async Task<IResult> FetchTopUpOptions([FromHeader(Name = "UserId")] int userId)
        {
            _log.LogInformation("Fetch TopUp Options");
            var result = await _topupService.GetTopUpOptions(userId);
            return Results.Ok(result);
        }

        [HttpPost("topup-beneficiary")]
        public async Task<IResult> TopUpBeneficiary([FromHeader(Name = "UserId")] int userId, TopUpBeneficiaryRequest req)
        {
            _log.LogInformation("Fetch TopUp Options");
            var result = await _topupService.TopUpBeneficiary(userId, req);
            return Results.Ok(result);
        }

    }
}
