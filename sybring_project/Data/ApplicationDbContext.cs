using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sybring_project.Models.Db;

namespace sybring_project.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Billing> Billings { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<TimeHistory> TimeHistories { get; set; } 

    }
}
