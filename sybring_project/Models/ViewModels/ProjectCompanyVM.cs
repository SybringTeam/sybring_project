using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class ProjectCompanyVM
    {
        public List<Project> ProjectVM { get; set; }

        public List<Company> CompanyVM { get; set; }

        public List<User> UserVM { get; set; }


    }
}
