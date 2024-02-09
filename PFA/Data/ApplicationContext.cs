using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace PFA.Data
{
    public class ApplicationContext :IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        //public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        //public DbSet<Post>? Posts { get; set; }
        //public DbSet<Page>? Pages { get; set; }
        //public DbSet<Setting>? Settings { get; set; }
    }
}
