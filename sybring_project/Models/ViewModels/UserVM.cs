using Microsoft.AspNetCore.Mvc.Rendering;
using sybring_project.Models.Db;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.ViewModels
{
    public class UserVM
    {
        public List<User> Users { get; set; }
        public List<Status> Statuss { get; set; }


    }
}
