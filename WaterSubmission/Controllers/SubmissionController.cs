using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WaterSubmission.Services.PricingService;
using Models;
using KubeMQ.SDK.csharp.Events.LowLevel;
using WaterSubmission.Data;
using KubeMQ.SDK.csharp.Tools;
using WaterSubmission.Services;
using MongoDB.Bson;

namespace WaterSubmission.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly IKubeMQSettings _mqSettings;
        private readonly ILogger<SubmissionController> _logger;
        private readonly IPricingService _pricingService;
        private readonly ISubmissionService _submissionService;

        private Sender sender;

        public SubmissionController(
            IKubeMQSettings mqSettings,
            ILogger<SubmissionController> logger,
            IPricingService pricingService,
            ISubmissionService submissionService)
        {
            _mqSettings = mqSettings;
            _logger = logger;
            _pricingService = pricingService;
            _submissionService = submissionService;
            sender = new Sender(_mqSettings.KubeMQServerAddress, _logger);
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] Submission submission)
        {
            var submissionPrice = await _pricingService.GetPrice(CreatePriceRequest(submission));
            submission.SubmissionPrice = submissionPrice;
            await _submissionService.SaveSubmission(submission);
            raiseSubmissionEvent(submission);
            
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Submission> getSubmission(ObjectId id)
        {
            return await _submissionService.GetSubmission(id);
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
