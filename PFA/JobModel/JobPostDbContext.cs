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
        //public DbSet<Post>? Posts { get; set; }
        //public DbSet<Page>? Pages { get; set; }
        //public DbSet<Setting>? Settings { get; set; }

    }
}
