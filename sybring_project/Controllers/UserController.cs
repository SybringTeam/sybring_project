﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
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
        private readonly IEmailSender _emailSender;


        public UserController(IUserServices userServices,
            UserManager<User> userManager, ApplicationDbContext applicationDbContext,
            IProjectServices projectServices, IEmailSender emailSender)
        {
            _userServices = userServices;
            _userManager = userManager;
            _projectServices = projectServices;
            _applicationDbContext = applicationDbContext;
            _emailSender = emailSender;
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
            var getUser = await _userServices.GetUserByIdAsync(userId);
            var project = await _userServices.GetProjectByIdAsync(projectId);

            if (getUser == null || project == null)
            {
                return NotFound("User or Project Not Found");

            }

            await _userServices.AssignProjectToUserAsync(userId, projectId);


            TempData["Added"] = "This Project has been assigned.";

            return RedirectToAction("Details", new { id = userId });
            //return PartialView("~/Views/Shared/_UserDetailsPartial.cshtml");

        }



        [HttpPost]
        public async Task<IActionResult> RemoveProject(string userId, int projectId)
        {
            var result = await _userServices.RemoveUserFromProjectAsync(projectId, userId);

            if (result)
            {
                TempData["Removed"] = "Project has been removed from the user.";
            }
            else
            {
                TempData["ErrorMessage"] = "Project removal failed. User or project not found.";
            }

            return RedirectToAction("Details", new { id = userId });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _userServices.DeleteUserAsync(id);
            return RedirectToAction("Index");
        }


        public IActionResult UserVc(string userId)
        {

            return ViewComponent("ShowUser", new { userId = userId });


        }



        // GET: UserController/AssignProjects
        [HttpGet]
        public async Task<IActionResult> AssignProjects()
        {
            var users = await _userServices.GetAllUserAsync();
            var projects = await _userServices.GetProjectsAsync();
            var viewModel = new AssignProjectsViewModel
            {
                Users = users,
                Projects = projects
            };
            return View(viewModel);
        }

        
        // POST: UserController/AssignProjects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjects(AssignProjectsViewModel viewModel)
        {
            foreach (var userId in viewModel.SelectedUserIds)
            {

                foreach (var projectId in viewModel.SelectedProjectIds)
                {
                    await _userServices.TaskManager(userId, projectId);
                   
                }
                TempData["Added"] = "This Project has been assigned.";

            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail()
        {

            var users = await _userServices.GetAllUserAsync();
            return View(users);


        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string userId, string subject, string htmlMessage)
        {
            var user = await _applicationDbContext.Users.FindAsync(userId);
            if (user == null)
            {
               
                return NotFound();
            }
            await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);

            return RedirectToAction("SendEmail");
        }


    }


}