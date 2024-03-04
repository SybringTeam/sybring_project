using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class ProjectUserVM
    {
        public List<Project> ProjectId { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<Project> ProjectName { get; set; }


        public int SelectedProjectId { get; set; }
    }
}
