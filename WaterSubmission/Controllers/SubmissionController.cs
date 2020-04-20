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

            var submitAccountingTask = _accountingControlService.SubmitAccounting(submission);
            var submitStatusTask = _statusControlService.SubmitStatus(submission);

            await Task.WhenAll(submitAccountingTask, submitStatusTask);

            if (submitAccountingTask.Result != System.Net.HttpStatusCode.OK){
                _logger.LogError($"Accounting control returned {submitAccountingTask.Result}");
                return BadRequest();
            }
            if (submitStatusTask.Result != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Status control returned {submitStatusTask.Result}");
                return BadRequest();
            }

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
