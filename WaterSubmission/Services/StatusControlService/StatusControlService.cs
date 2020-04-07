using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WaterSubmission.Services.StatusControlService
{
    public class StatusControlService : IStatusControlService
    {
        private readonly string BaseUrl;

        private readonly HttpClient _httpClient;

        public StatusControlService(
            HttpClient httpClient,
            IConfiguration config)
        {
            BaseUrl = config.GetValue<string>("BaseUrls:AccountingControlBaseUrl");

            _httpClient = httpClient;
        }

        public async Task<HttpStatusCode> SubmitStatus(Submission submission){

            var jsonSubmission = JsonConvert.SerializeObject(submission);
            var content = new StringContent(jsonSubmission, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl, content);

            return response.StatusCode;
        }
    }
}
