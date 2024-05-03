using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Areas.Identity.Pages.Account.Manage
{
    public class UserOverViewModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IUserServices _userServices;

        public UserOverViewModel(UserManager<User> userManager, ApplicationDbContext db,
            IUserServices userServices)
        {
            _userManager = userManager;
            _db = db;
            _userServices = userServices;
        }

        public User GetUser { get; set; }
        public List<Project> AssignedProjects { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            GetUser = await _userManager.GetUserAsync(User);

            if (User == null)
            {
                return NotFound("User not found");
            }

            AssignedProjects = await _db.Projects
                .Include(p => p.Users)
                .Where(p => p.Users.Any(p=> p.Id == GetUser.Id))
                .ToListAsync();
               
            return Page();
        }

    }

}

