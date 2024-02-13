using Microsoft.AspNetCore.Identity;
using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class IdentityUserRole : IdentityUserRole<string>
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public IEnumerable<string>? Roles { get; set; }
       
    }
}
