﻿using Microsoft.AspNetCore.Mvc;
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
        [Route("pd")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectServices.GetProjectByIdAsync(id);

         
            if (project.TimeId == null || !project.TimeId.Any())
            {
                ViewBag.NoTimeHistoryMessage = "New user has no time to show.";
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
