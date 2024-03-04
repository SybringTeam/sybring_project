using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class AssignProjectViewModel
    {
        public User User { get; set; }
        public List<Project> Projects { get; set; }
        public int SelectedProjectId { get; set; }
    }
}
