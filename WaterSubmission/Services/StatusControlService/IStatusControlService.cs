using Models;
using System.Net;
using System.Threading.Tasks;

namespace WaterSubmission.Services.StatusControlService
{
    public interface IStatusControlService
    {
        Task<HttpStatusCode> SubmitStatus(Submission submission);
    }
}
