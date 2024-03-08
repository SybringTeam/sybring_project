using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;

namespace sybring_project.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectServices _projectServices;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserServices _userServices;

        public ProjectController(IProjectServices projectServices,
           ApplicationDbContext applicationDbContext,
           IUserServices userServices)
        {
            _applicationDbContext = applicationDbContext;
            _projectServices = projectServices;
            _userServices = userServices;
        }


     
        public async Task<IActionResult> Index()
        {
            var projectsList = await _projectServices.GetProjectsAsync();
            return View(projectsList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectServices.GetProjectByIdAsync(id);


            if (project.TimeId == null || !project.TimeId.Any())
            {
                ViewBag.NoTimeHistoryMessage = "New user has no time to show.";
            }

            var allUsers = await _applicationDbContext.Users.ToListAsync();

            if (allUsers != null)
            {
                ViewBag.AllUsers = allUsers;
            }

            return View(project);
        }

        [HttpPost]
        
        public async Task<IActionResult> Details(string userId, int projectId)
        {

            try
            {
                var getProject = await _projectServices.GetProjectByIdAsync(projectId);
                var getUser = await _userServices.GetUserByIdAsync(userId);

                if (getUser == null || getProject == null)
                {
                    return NotFound("User or Project Not Found");
                }

                await _projectServices.AssigUserToProjectAsync(userId, projectId);

                TempData["Added"] = "This User has been assigned to the project.";

                return RedirectToAction("Details", new { id = projectId });
            }
            catch (InvalidOperationException ex)
            {
                // Log the exception or handle it as needed
                return NotFound(ex.Message);
            }


        }



      
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Route("pc")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            await _projectServices.AddProjectAsync(project);
            return RedirectToAction("Index");


        }

        public async Task<IActionResult> Delete(int id)
        {
            await _projectServices.DeleteProjectAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectId = await _projectServices.GetProjectByIdAsync(id);

            if (projectId == null)
            {
                return NotFound();
            }

            return View(projectId);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Project project)
        {
            await _projectServices.UpdateProjectAsync(project);
            return RedirectToAction("Index");

        }



    }
}
