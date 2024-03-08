using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;

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


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<User>()
                .HasMany(u => u.ProjectId)
                .WithMany(u => u.Users)
                .UsingEntity(j => j.ToTable("ProjectUsers"));


            builder.Entity<Project>()
                .HasMany(p => p.Users)
                .WithMany(p => p.ProjectId)
                .UsingEntity(t => t.ToTable("ProjectUsers"));

            builder.Entity<TimeHistory>()
             .HasMany(u => u.Users)
             .WithMany(u => u.TimeId)
             .UsingEntity(j => j.ToTable("TimeHistoryUser"));

            base.OnModelCreating(builder);
        }







    }
}
