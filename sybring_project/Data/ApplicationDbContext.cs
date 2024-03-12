using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using System.Reflection.Emit;

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

        public DbSet<ProjectTimeReport> ProjectTimeReport { get; set; }


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
     .HasMany(t => t.Users)
     .WithMany(u => u.TimeId)
     .UsingEntity<Dictionary<string, object>>(
         "TimeHistoryUser",
         j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
         j => j.HasOne<TimeHistory>().WithMany().HasForeignKey("TimeId")
     );


          


            base.OnModelCreating(builder);
        }







    }
}
