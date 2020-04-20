using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WaterSubmission.Services.StatusControlService
{
    public class MockStatusControlService : IStatusControlService
    {
        public Task<HttpStatusCode> SubmitStatus(Submission submission)
        {
            return Task.FromResult(HttpStatusCode.OK);
        }
    }
}
