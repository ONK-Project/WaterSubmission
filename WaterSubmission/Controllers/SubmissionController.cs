using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WaterSubmission.Services.AccountingControlService;
using WaterSubmission.Services.PricingService;
using WaterSubmission.Services.StatusControlService;
using Models;
using KubeMQ.SDK.csharp.Events.LowLevel;
using Microsoft.Extensions.Configuration;
using WaterSubmission.Data;
using KubeMQ.SDK.csharp.Tools;

namespace WaterSubmission.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly IAccountControlKubeMQSettings _mqSettings;
        private readonly ILogger<SubmissionController> _logger;
        private readonly IAccountingControlService _accountingControlService;
        private readonly IStatusControlService _statusControlService;
        private readonly IPricingService _pricingService;
        private Sender sender;

        public SubmissionController(
            IAccountControlKubeMQSettings mqSettings,
            ILogger<SubmissionController> logger, 
            IAccountingControlService accountingControlService,
            IStatusControlService statusControlService,
            IPricingService pricingService)
        {
            _mqSettings = mqSettings;
            _logger = logger;
            _accountingControlService = accountingControlService;
            _statusControlService = statusControlService;
            _pricingService = pricingService;

            sender = new Sender(_mqSettings.KubeMQServerAddress, _logger);
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] Submission submission)
        {
            var submissionPrice = await _pricingService.GetPrice(CreatePriceRequest(submission));
            submission.SubmissionPrice = submissionPrice;

            raiseSubmissionEvent(submission);

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

        void raiseSubmissionEvent(Submission submission)
        {
            Event @event = new Event()
            {
                Body = Converter.ToByteArray(submission),
                Store = true,
                Channel = _mqSettings.ChannelName,
                ClientID = _mqSettings.ClientID,
                ReturnResult = false
            };
            sender.SendEvent(@event);
        }
    }
}
