using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ProjectTimeReport>? ProjectHistories { get; set; }
        public virtual ICollection<User>? Users { get; set; }


    }
}