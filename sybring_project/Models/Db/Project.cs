using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<TimeHistory> TimeId { get; set; }
        public virtual ICollection<User>? Users { get; set;}
    }
}
