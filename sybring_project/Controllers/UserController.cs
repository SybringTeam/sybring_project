﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;

        public UserController(IUserServices userServices,
            ApplicationDbContext applicationDbContext, UserManager<User> userManager)
        {
            _userServices = userServices;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        [Route("in")]
        public async Task<IActionResult> Index()
        {
            var users = await _userServices.GetAllUserAsync();
            return View(users);
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
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await _userServices.AddUsersAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _userServices.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userServices.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
