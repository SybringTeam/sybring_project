using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IProjectServices _projectServices;


        public UserController(IUserServices userServices,
            UserManager<User> userManager, ApplicationDbContext applicationDbContext,
            IProjectServices projectServices)
        {
            _userServices = userServices;
            _userManager = userManager;
            _projectServices = projectServices;
            _applicationDbContext = applicationDbContext;
        }

        [Route("ui")]
        public async Task<IActionResult> Index()
        {
            var list = await _userServices.GetAllUserAsync();
            return View(list);
        }

        [HttpGet]
        [Route("uc")]
        public async Task<IActionResult> Create()
        {
            var projects = await _userServices.GetProjectsAsync();

            if (projects == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Projects = projects;
            return View();
        }

        [HttpPost]
        [Route("uc")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, int projectId)
        {

            await _userServices.AddUsersAsync(user, projectId);
            return RedirectToAction("Index");

        }

        [Route("ue")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Route("ue")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {

            await _userServices.UpdateUserAsync(user);
            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = await _userServices.GetUserByIdAsync(id);

            if (detail == null)
            {
                return NotFound();
            }

            var allProjects = await _applicationDbContext.Projects.ToListAsync();

            if (allProjects!= null) 
            {
                ViewBag.AllProjects = allProjects;
            }
           


            return View(detail);
        }

        [HttpPost]
        public async Task<IActionResult> Details(string userId, int projectId)
        {
            if (userId == null || projectId == 0)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            var project = await _applicationDbContext.Projects.FindAsync(projectId);

            if (project != null)
            {
                user.ProjectId = new List<Project> { project };
                await _applicationDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = userId });
        }




        public async Task<IActionResult> Delete(string id)
        {
            await _userServices.DeleteUserAsync(id);
            return RedirectToAction("Index");
        }



      
    }
}