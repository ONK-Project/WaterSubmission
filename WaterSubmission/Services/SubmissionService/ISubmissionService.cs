using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterSubmission.Services
{
    public interface ISubmissionService
    {
        Task<Submission> GetSubmission(int id);
        Task SaveSubmission(Submission submission);
        bool CreateDB();
    }
}
