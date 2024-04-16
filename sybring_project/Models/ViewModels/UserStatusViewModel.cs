using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class UserStatusViewModel
    {
        public IEnumerable<User>? Users { get; set; }
        public IEnumerable<Status>? Statuses { get; set; }
    }
}
