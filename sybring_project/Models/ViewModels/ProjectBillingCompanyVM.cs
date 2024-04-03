using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class ProjectBillingCompanyVM
    {
        public IEnumerable<Project>? Projects { get; set; }

        public IEnumerable<Company>? Companies { get; set; }

        public IEnumerable<User>? Users { get; set; }


    }
}
