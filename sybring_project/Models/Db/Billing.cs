using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.Db
{
    public class Billing
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateStamp { get; set; } = DateTime.Now;
        public string? ImageLink { get; set; }

        public string Description { get; set; } = string.Empty;
        
        public double Cost { get; set; }

        [NotMapped]
        public Uri? BlobLink { get; set; }
       
        public string? SelectedUserId { get; set; }
        public virtual ICollection<Project> ProjectId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
