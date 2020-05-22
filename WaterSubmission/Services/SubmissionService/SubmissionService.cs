using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterSubmission.Data;

namespace WaterSubmission.Services.SubmissionService
{
    public class SubmissionService : ISubmissionService
    {
        private DbContextOptions<SubmissionDbContext> _options;

        public SubmissionService(string connectionString)
        {
            _options = new DbContextOptionsBuilder<SubmissionDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public async Task<Submission> GetSubmission(int id)
        {
            using (var context = new SubmissionDbContext(_options))
            {
                return await context.Submissions.FindAsync(id);
            }
        }

        public async Task SaveSubmission(Submission submission)
        {
            using (var context = new SubmissionDbContext(_options))
            {
                await context.Submissions.AddAsync(submission);
                await context.SaveChangesAsync();
            }
        }

        public bool CreateDB()
        {
            using (var context = new SubmissionDbContext(_options))
            {
                if (true && (context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return false;

                context.Database.EnsureDeleted();
                return context.Database.EnsureCreated();
            }
        }
    }
}
