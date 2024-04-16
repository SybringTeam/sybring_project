using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sybring_project.Data;

using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;

namespace sybring_project.Controllers
{
    public class BillingController : Controller
    {
        private readonly IProjectServices _projectServices;
        private readonly IUserServices _userServices;
        private readonly IBillingServices _billingServices;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;

        public BillingController(IProjectServices projectServices, IUserServices userServices,
            IBillingServices billingServices, ApplicationDbContext applicationDbContext,
            UserManager<User> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _projectServices = projectServices;
            _billingServices = billingServices;
            _userServices = userServices;
            _userManager = userManager;
        }


        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult>Index()
        {
            var userId = _userManager.GetUserId(User);
            var billingList = await _billingServices.GetBillingAsync(userId);
            return View(billingList);
        }

        [HttpGet]
        [Authorize(Roles = "admin, superadmin, underconsult")]
        public async Task<IActionResult> Create() 
        {
            var viewModel = await _billingServices.GetProjectsAndUsersAsync();
            return View(viewModel);
                        
        }

        [HttpPost]
        [Authorize(Roles = "admin, superadmin, underconsult")]
        public async Task<IActionResult> Create(BillingVM billingVM, int projectId, string selectedUserId)
        {
            //billingVM.ImageLink = Guid.NewGuid().ToString() + "_" + billingVM.File.FileName;
            //await _billingServices.UploadImageFileAsync(billingVM);

            var adminUserId = _userManager.GetUserId(User);
                        
          
            await _billingServices.AddBillingAsync(billingVM, selectedUserId, projectId);
            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> Details(int id) 
        {
            var byId = await _billingServices.GetBillingByIdAsync(id);
            return View(byId);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            await _billingServices.DeleteBillingAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Edit(int id)
        {
            var billingEdit = await _billingServices.GetBillingByIdAsync(id);
          
            return View(billingEdit);
        }

        [HttpPost]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Edit(Billing billing)
        {
            await _billingServices.UpdateCompanyAsync(billing);
            return RedirectToAction("Index");
        }

    

    }
}
