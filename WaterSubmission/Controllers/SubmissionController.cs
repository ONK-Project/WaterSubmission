using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WaterSubmission.Services.AccountingControlService;
using WaterSubmission.Services.PricingService;
using WaterSubmission.Services.StatusControlService;
using Models;

namespace WaterSubmission.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ILogger<SubmissionController> _logger;
        private readonly IAccountingControlService _accountingControlService;
        private readonly IStatusControlService _statusControlService;
        private readonly IPricingService _pricingService;

        public SubmissionController(
            ILogger<SubmissionController> logger, 
            IAccountingControlService accountingControlService,
            IStatusControlService statusControlService,
            IPricingService pricingService)
        {
            _logger = logger;
            this._accountingControlService = accountingControlService;
            this._statusControlService = statusControlService;
            this._pricingService = pricingService;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] Submission submission)
        {
            var submissionPrice = await _pricingService.GetPrice(CreatePriceRequest(submission));
            submission.SubmissionPrice = submissionPrice;

            Task submitAccountingTask = _accountingControlService.SubmitAccounting(submission);
            Task submitStatusTask = _statusControlService.SubmitStatus(submission);

            await Task.WhenAll(submitAccountingTask, submitStatusTask);

            return Ok();
        }

        private PriceRequest CreatePriceRequest(Submission submission) {
            var priceRequest = new PriceRequest()
            {
                RessourceUsage = submission.RessourceUsage,
                DateTime = submission.DateTime,
                UnitOfMeassure = submission.UnitOfMeassure
            };

            return priceRequest;
        }
    }
}
