using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
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
        public async Task<IActionResult> Create(User user)
        {
           
                await _userServices.AddUsersAsync(user);
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
            var detail = await _userServices.GetUserByIdAsync(id);
            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _userServices.DeleteUserAsync(id);
             return RedirectToAction("Index");
        }

     
        
    }
}