using Models;
using System.Threading.Tasks;

namespace WaterSubmission.Services.PricingService
{
    public interface IPricingService
    {
        Task<SubmissionPrice> GetPrice(PriceRequest priceRequest);
    }
}
