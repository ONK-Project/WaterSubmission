﻿
using Microsoft.Extensions.Configuration;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WaterSubmission.Data;

namespace WaterSubmission.Services.SubmissionService
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IMongoCollection<Submission> _submissions;

        public SubmissionService(ISubmissionDbSettings submissionDbSettings)
        {
            var client = new MongoClient(submissionDbSettings.ConnectionString);
            var database = client.GetDatabase(submissionDbSettings.DatabaseName);
            _submissions = database.GetCollection<Submission>(submissionDbSettings.SubmissionCollectionName);
        }

        public async Task<Submission> GetSubmission(string id)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - Get submission using id: " + id);
            var filter = new ExpressionFilterDefinition<Submission>(s => s.SubmissionId == id);
            var submission = await _submissions.FindAsync(filter);
            return submission.First();
        }

        public Task SaveSubmission(Submission submission)
        {
            var savedSubmission = _submissions.InsertOneAsync(submission);
            Console.WriteLine(DateTime.Now.ToString() + " - Submission saved with id: " + submission.SubmissionId);
            return savedSubmission;
        }
    }
}
