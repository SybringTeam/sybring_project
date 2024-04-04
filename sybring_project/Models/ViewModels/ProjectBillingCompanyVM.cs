using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class ProjectBillingCompanyVM
    {
        public List<Project>? Projects { get; set; }

        public List<Company>? Companies { get; set; }

        public List<User>? Users { get; set; }


    }
}
