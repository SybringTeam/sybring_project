using sybring_project.Models.Db;
using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.ViewModels
{
    public class AssignProjectsViewModel
    {
        [Display(Name = "Select Users")]
        public List<User> Users { get; set; }

        [Display(Name = "Select Projects")]
        public List<Project> Projects { get; set; }

        [Display(Name = "Selected User IDs")]
        public List<string> SelectedUserIds { get; set; }

        [Display(Name = "Selected Project IDs")]
        public List<int> SelectedProjectIds { get; set; }
    }
}
