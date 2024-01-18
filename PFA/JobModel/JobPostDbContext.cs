using Microsoft.EntityFrameworkCore;

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
    }
}
