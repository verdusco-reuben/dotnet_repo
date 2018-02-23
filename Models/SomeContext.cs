using Microsoft.EntityFrameworkCore;

namespace beltexam.Models
{
    public class SomeContext : DbContext
    {
        public SomeContext(DbContextOptions<SomeContext> options) : base(options) { }
        public DbSet<User> users { get ; set ; }
        public DbSet<Activity> activities { get ; set ; }
        public DbSet<UserActivity> useractivities { get ; set ;}
    }
}