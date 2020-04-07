using Models;
using System.Net;
using System.Threading.Tasks;

namespace WaterSubmission.Services.AccountingControlService
{
    public interface IAccountingControlService
    {
        Task<HttpStatusCode> SubmitAccounting(Submission submission);
    }
}
