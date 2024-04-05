using Microsoft.EntityFrameworkCore;
using PFA.Models;

namespace PFA.JobModel
{
    public class JobPostDbContext :DbContext
    {
        public JobPostDbContext()
        {
        }

        public JobPostDbContext(DbContextOptions<JobPostDbContext>options):base(options) { 
        
        }
        public DbSet<JobPostModel> JobPosts { get; set; }

        public DbSet<FileModel> Files { get; set; }



    }
}
