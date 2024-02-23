using Microsoft.AspNetCore.Mvc;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectServices _projectServices;
        private readonly ApplicationDbContext _applicationDbContext;

        public ProjectController(IProjectServices projectServices,
           ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _projectServices = projectServices;
        }


        [Route("pi")]
        public async Task<IActionResult> Index()
        {
            var projectsList = await _projectServices.GetProjectsAsync();
            return View(projectsList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectServices.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [Route("pc")]
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

    }
}
