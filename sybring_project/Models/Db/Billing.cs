using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db
{
    public class Billing
    {
        
        public int Id { get; set; }

        public string Image { get; set; }

        public string Description { get; set; } = string.Empty;
        
        public double Cost { get; set; }

        public virtual ICollection<Project> ProjectId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
