using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace WaterSubmission.Services.PricingService
{
    public class PricingService : IPricingService
    {
        private readonly string BaseUrl;

        private readonly HttpClient _httpClient;

        public PricingService(
            HttpClient httpClient, 
            IConfiguration config)
        {
            BaseUrl = config.GetValue<string>("BaseUrls:PricingControlBaseUrl");
            _httpClient = httpClient;
        }

        public async Task<SubmissionPrice> GetPrice(PriceRequest priceRequest) 
        {
            var response = await _httpClient.GetAsync(CreateGetUrl(priceRequest));
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var submissionPrice = JsonConvert.DeserializeObject<SubmissionPrice>(responseBody);

            return submissionPrice;
        }

        private string CreateGetUrl(PriceRequest priceRequest) 
        {
            var getUrl = BaseUrl + "? ressourceusage = [RessourceUsage] & datetime = [DateTime] & unitofmeassure = [UnitOfMeassure]";
            getUrl = getUrl.Replace("[RessourceUsage]", priceRequest.RessourceUsage.ToString());
            getUrl = getUrl.Replace("[DateTime]", priceRequest.DateTime.ToString());
            getUrl = getUrl.Replace("[UnitOfMeassure]", priceRequest.UnitOfMeassure);

            return getUrl;
        }
    }
}
