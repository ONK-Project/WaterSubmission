using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterSubmission.Data
{
    public interface ISubmissionDbSettings
    {
        string SubmissionCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class SubmissionDbSettings : ISubmissionDbSettings
    {
        public string SubmissionCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
