using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.Db
{
    public class Company
    {
        [ForeignKey("Project")]
        [Key]
        public int Id { get; set; }

        public string? SupervisorName { get; set; }
        public string? CompanyWeb { get; set; }

        public int SupervisorPhone { get; set; }

        public string? SupervisorEmail { get; set; }

        public double? OrgNumber { get; set; }

        public string? CompanyAddress { get; set; } = null;
        [ForeignKey("Company")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

       




    }
}
