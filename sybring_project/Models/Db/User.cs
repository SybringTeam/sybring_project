using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.Db
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? CitizenMembership { get; set; }
        public int? Age { get; set; }

        public DateTime? DOB { get; set; }

        public string? Address { get; set; } = string.Empty;

        public string? TaskDescription { get; set; } = string.Empty;

        public double UserIncome { get; set; }

        public double UserPersonalNumber { get; set; }
        public string? ICEContactName { get; set; }
        public string? UserICE { get; set; }

        public string? Seller { get; set; }
        public string ImageLink { get; set; } = string.Empty;

        [NotMapped]
        public Uri? BlobLink { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }

        // Navigation properties
        
        public virtual ICollection<Status>? Status { get; set; } = new List<Status>();
        public virtual ICollection<Project>? ProjectId { get; set; }

        public virtual ICollection<TimeHistory>? TimeId { get; set; }

        public virtual ICollection<Billing>? ReceiptId { get; set; }



    }
}