using Models;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace WaterSubmission.Services
{
    public interface ISubmissionService
    {
        Task<Submission> GetSubmission(string id);
        Task SaveSubmission(Submission submission);
    }
}
