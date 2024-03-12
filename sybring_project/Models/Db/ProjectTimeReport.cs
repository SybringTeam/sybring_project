using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db
{
    public class ProjectTimeReport
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public Project Projects { get; set; }
        public int TimeId { get; set; }
        public TimeHistory TimeHistory { get; set; }

        public TimeSpan ProjectHours { get; set; }




    }
}
