using sybring_project.Models.Db;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.ViewModels
{
    public class BillingVM
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateTime DateStamp { get; set; } = DateTime.Now;
        public string? ImageLink { get; set; }

        public string Description { get; set; } = string.Empty;

        public double Cost { get; set; }

        public string UserId { get; set; }

        public string? SelectedUserId { get; set; }
        public IFormFile? File { get; set; }
        public virtual ICollection<Project> ProjectId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
