using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string? CompanyWeb { get; set; }

        public int CompanyContact { get; set; }

        public string? CompanyEmail { get; set; }

        public double? OrgNumber { get; set; }
        public string? CompanyAdress { get; set; } = null;


    }
}
