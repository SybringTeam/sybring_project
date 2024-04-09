using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.Db
{
    public class Project
    {
        [Key]
        
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Project")]
        public int CompanyId { get; set; }
     
        public int? BillingId { get; set; }

        [NotMapped]
        public IFormFile? Contracts { get; set; }

      public virtual Billing? Billing { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<ProjectTimeReport>? ProjectHistories { get; set; }
        public virtual ICollection<User>? Users { get; set; }


    }
}