using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WaterSubmission.Services.AccountingControlService
{
    public class AccountingControlService : IAccountingControlService
    {
        private readonly string BaseUrl;

        private readonly HttpClient _httpClient;

        public AccountingControlService(
            HttpClient httpClient,
            IConfiguration config)
        {
            BaseUrl = config.GetValue<string>("BaseUrls:AccountingControlBaseUrl");

            _httpClient = httpClient;
        }

        public async Task<HttpStatusCode> SubmitAccounting(Submission submission) {

            var jsonSubmission = JsonConvert.SerializeObject(submission);
            var content = new StringContent(jsonSubmission, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl, content);

            return response.StatusCode;
        }
    }
}
