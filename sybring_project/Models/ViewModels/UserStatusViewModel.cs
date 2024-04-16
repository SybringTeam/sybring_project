using Microsoft.AspNetCore.Mvc.Rendering;
using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class UserStatusViewModel
    {
        //public UserStatusViewModel()
        //{
        //    Statuses = new List<SelectListItem>();
        //}
        public IEnumerable<User>? Users { get; set; }
       public IEnumerable<Status>? Statuses { get; set; }

        //public List<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
    }
}
