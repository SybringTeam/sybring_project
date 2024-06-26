﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;
using System.Runtime.InteropServices;

namespace sybring_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IProjectServices _projectServices;
        private readonly IEmailSender _emailSender;
        private readonly IStatusService _statusService;


        public UserController(IUserServices userServices,
            UserManager<User> userManager, ApplicationDbContext applicationDbContext,
            IProjectServices projectServices, IEmailSender emailSender,
            IStatusService statusService, RoleManager<IdentityRole> roleManager)
        {
            _userServices = userServices;
            _userManager = userManager;
            _projectServices = projectServices;
            _applicationDbContext = applicationDbContext;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _statusService = statusService;
        }

        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Index()
        {
            var userListUK = await _userServices.GetAllUsersInRoleAsync("underconsult");
            var allStatuses = await _userServices.GetStatusListAsync();


            ViewBag.Statuses = allStatuses;


            return View(userListUK);
        }



        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> RoleManager()
        {
            var userList = await _userServices.GetAllUserAsync();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in userList)

            {
                var roles = await _userManager.GetRolesAsync(user);
                var rolesList = roles.ToList();
                rolesList.Sort();
                userRoles.Add(user.Id, rolesList);
            }

            ViewBag.UserRoles = userRoles;

            return View(userList);
        }


        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            if (!User.IsInRole("superadmin"))
            {
                return Forbid();
            }
            if (newRole != "underconsult" && newRole != "admin" && newRole != "archive")
            {
                return BadRequest("Invalid role");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (await _userManager.IsInRoleAsync(user, "superadmin"))
            {
                return BadRequest("Cannot change role for superadmin");
            }
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.AddToRoleAsync(user, newRole);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "admin, superadmin")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string userId, int statusId)
        {
            await _userServices.AddStatusToUserAsync(userId, statusId);
            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> RemoveStatusToUser(string userId, int statusId)
        {
            await _userServices.RemoveStatusFromUserAsync(userId, statusId);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> RoleView(string roleName)
        {
            ViewBag.RoleName = roleName; // Pass the roleName to the view

            if (roleName != "admin")  // Check if the requested role is not "admin"
            {
                var list = await _userServices.GetAllUsersInRoleAsync(roleName);
                return View(list);
            }
            else  // Handle the case when the requested role is "admin"
            {
                // Fetch users with both "Admin" and "SuperAdmin" roles
                var adminUsers = await _userServices.GetAllUsersInRoleAsync("Admin");
                var superAdminUsers = await _userServices.GetAllUsersInRoleAsync("SuperAdmin");

                // Combine the users from both roles
                var combinedUsers = adminUsers.Concat(superAdminUsers).ToList();

                return View(combinedUsers);
            }
        }

        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserByIdAsync(id);


            return View(user);
        }

        [Authorize(Roles = "admin, superadmin")]
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


            //TempData["Added"] = "This Project has been assigned.";

            return RedirectToAction("Details", new { id = userId });
            //return PartialView("~/Views/Shared/_UserDetailsPartial.cshtml");

        }



        [HttpPost]
        [ActionName("RemoveProjects")]
        public async Task<IActionResult> RemoveProjectsPost(string userId, List<int> projectIds)
        {
            foreach (var projectId in projectIds)
            {
                var result = await _userServices.RemoveUserFromProjectAsync(projectId, userId);

                if (!result)
                {
                    TempData["ErrorMessage"] = "One or more projects could not be removed. Please try again.";
                    return RedirectToAction("Index", new { id = userId });
                }

            }

            TempData["Removed"] = "Selected projects have been removed from the user.";

            return RedirectToAction("Index", new { id = userId });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(user, "underconsult"))
            {
                await _userManager.RemoveFromRoleAsync(user, "underconsult");
                await _userManager.AddToRoleAsync(user, "archive");
            }
            else if (await _userManager.IsInRoleAsync(user, "archive"))
            {
                await _userManager.RemoveFromRoleAsync(user, "archive");
                await _userManager.AddToRoleAsync(user, "underconsult");
            }
            else
            {
                // User is not in the "underconsult" role, handle this case as needed
                // For example, return an error message or handle it differently
                // You may also choose to do nothing in this case if it's not an error
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TrueDelete(string id)
        {
            await _userServices.DeleteUserAsync(id);
            return RedirectToAction("Index");
        }


        public IActionResult UserVc(string userId)
        {

            return ViewComponent("ShowUser", new { userId = userId });


        }

        // GET: UserController/AssignProjects
        [Authorize(Roles = "admin, superadmin")]
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
        [Authorize(Roles = "admin, superadmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjects(AssignProjectsViewModel viewModel)
        {
            foreach (var userId in viewModel.SelectedUserIds)
            {

                foreach (var projectId in viewModel.SelectedProjectIds)
                {
                    await _userServices.TaskManager(userId, projectId);
                    // Get the user's email address
                    var user = await _userServices.GetUserByIdAsync(userId);
                    var userEmail = user.Email;

                    // Get the project's name
                    var project = await _userServices.GetProjectByIdAsync(projectId);
                    var projectName = project.Name;

                    await _emailSender.SendEmailAsync(userEmail, "You've been assigned to a project",
                    $"Hello {user.FirstName},\n\nYou've been assigned to the project: " +
                    $"{projectName}.\n\n" +
                    $"Regards,\n\n " +
                    $"Sybring AB");


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




        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("EmailConfirmed");
            }
            else
            {
                return View("Error");
            }
        }

    }


}