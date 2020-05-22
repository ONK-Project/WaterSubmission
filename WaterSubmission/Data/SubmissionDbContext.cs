using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterSubmission.Data
{
    public class SubmissionDbContext : DbContext
    {
        public DbSet<Submission> Submissions { get; set; }

        public SubmissionDbContext() { }

        public SubmissionDbContext(DbContextOptions<SubmissionDbContext> options)
           : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            onModelCreatingSubmission(modelBuilder);
            onModelCreatingSeedData(modelBuilder);
        }

        private void onModelCreatingSubmission(ModelBuilder modelBuilder) { }

        private void onModelCreatingSeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Submission>().HasData(
                new Submission
                {
                    SubmissionId = 99999999,
                    DateTime = DateTime.Now,
                    RessourceUsage = 69,
                    UnitOfMeassure = "m^3",
                });
        }
    }
}
