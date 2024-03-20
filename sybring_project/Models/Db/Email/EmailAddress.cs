using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db.Email
{
    public class EmailAddress
    {
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Address { get; set; } = string.Empty;
    }
}
