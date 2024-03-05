using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using System.Runtime.InteropServices;

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
        
        public async Task<IActionResult> Index()
        {
            var list = await _userServices.GetAllUserAsync();
            return View(list);
        }

        [HttpGet]

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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, int projectId)
        {

            await _userServices.AddUsersAsync(user, projectId);
            return RedirectToAction("Index");

        }


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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {

            await _userServices.UpdateUserAsync(user);
            return RedirectToAction("Index");

        }

        [HttpGet]

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

            if (allProjects != null)
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

            var projectList = _projectServices.GetProjectsAsync();
            var projects = await projectList ?? new List<Project>();
            var pro = projects.FirstOrDefault(p => p.Id == projectId);


            var project = await _applicationDbContext.Projects.FindAsync(projectId);
           
            if (pro != null)
            {
                user.ProjectId = user.ProjectId ?? new List<Project>();
                user.ProjectId.Add(project);

                var existingProj = _applicationDbContext.Users
                    .Any(u => u.Id == user.Id && u.ProjectId
                    .Any(uc => uc.Id == pro.Id));

                if (existingProj)
                {
                    TempData["ErrorMessage"] = "This Project is Already Assigned";
                    return RedirectToAction("Details", new { id = userId });
                }
               
                await _applicationDbContext.SaveChangesAsync();
                TempData["Added"] = "This Project has been assigned.";
            }
            

            return RedirectToAction("Details", new { id = userId });
        }


        [HttpPost]
        public async Task<IActionResult> RemoveProject(string userId, int projectId)
        {
           
            var user = await _userManager.FindByIdAsync(userId);
            var project = await _applicationDbContext.Projects.FindAsync(projectId);

            if (user != null && project != null)
            {
              
                user.ProjectId?.Remove(project);
                await _applicationDbContext.SaveChangesAsync();
            }

          
            return RedirectToAction("Details");
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _userServices.DeleteUserAsync(id);
            return RedirectToAction("Index");
        }
    }
}