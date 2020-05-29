using Models;
using System.Threading.Tasks;

namespace WaterSubmission.Services
{
    public interface ISubmissionService
    {
        Task<Submission> GetSubmission(int id);
        Task SaveSubmission(Submission submission);
    }
}
