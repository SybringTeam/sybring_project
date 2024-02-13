using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Identity;

namespace sybring_project.Models.Db
{
    public class User : IdentityUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }= string.Empty;

        public int Age { get; set; }

        public string Address { get; set; } = string.Empty;

        public string TaskDescription { get; set; } = string.Empty;

        
        public virtual ICollection<Project>? ProjectId { get; set; }

        public virtual ICollection<TimeHistory>? TimeId { get; set; }

        public virtual ICollection<Billing>? ReceiptId { get; set; }


    }
}
