using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db
{
    public class TimeHistory
    {
        
        public int Id { get; set; }

        public DateTime DateTime { get; set; }


        // Represents many-to-many relationship with Project
        public virtual ICollection<Project> ProjectId { get; set; }


        // Represents many-to-many relationship with User
        public virtual ICollection<User> Users { get; set; }
    }
}
