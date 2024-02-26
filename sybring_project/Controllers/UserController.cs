using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using System.Runtime.ConstrainedExecution;

namespace sybring_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserServices userServices, 
            UserManager<User> userManager)
        {
            _userServices = userServices;
            _userManager = userManager;
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User newUser, string password) 
        {
            var newUSer = new User
            {

                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                PasswordHash = newUser.PasswordHash

            };

             var result = await _userServices.RegisterUserAsync(newUSer, password);

            if (result != null)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Please check your information.");
            return View(newUser);
        }
     
        
    }
}