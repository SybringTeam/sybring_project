using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.Db
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }= string.Empty;

        public int Age { get; set; }

        public string Address { get; set; } = string.Empty;

        public string TaskDescription { get; set; } = string.Empty;

        public string ImageLink { get; set; } = string.Empty;

        [NotMapped]
        public Uri? BlobLink { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public virtual ICollection<Project>? ProjectId { get; set; }

        public virtual ICollection<TimeHistory>? TimeId { get; set; }

        public virtual ICollection<Billing>? ReceiptId { get; set; }


    }
}
