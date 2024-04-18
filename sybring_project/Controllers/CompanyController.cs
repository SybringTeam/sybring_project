using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyServices _companyServices;
        private readonly IProjectServices _projectServices;
        private readonly ApplicationDbContext _applicationDbContext;

        public CompanyController(IProjectServices projectServices, ICompanyServices 
            companyServices, ApplicationDbContext applicationDbContext)
        {

            _projectServices = projectServices;
            _applicationDbContext = applicationDbContext;
            _companyServices = companyServices;
        }
        public async Task<IActionResult> Index()
        {
            var companyList = await _companyServices.GetCompanyAsync();

            return View(companyList);
        }

        [Authorize(Roles = "admin, superadmin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectServices.GetProjectsAsync();
            ViewBag.Projects = projects;

            return View();
        }

        [Authorize(Roles = "admin, superadmin")]
        [HttpPost]
        public async Task<IActionResult> Create(Company company)
        {
            await _companyServices.AddCompanyAsync(company);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, superadmin")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var byId = await _companyServices.GetCompanyByIdAsync(id);
            return View(byId);
        }


        [Authorize(Roles = "admin, superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyServices.DeleteCompanyAsync(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, superadmin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id) 
        {
            var companyEdit = await _companyServices.GetCompanyByIdAsync(id);
            return View(companyEdit);
        }

        [Authorize(Roles = "admin, superadmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Company company) 
        {
            await _companyServices.UpdateCompanyAsync(company);
            return RedirectToAction("Index");
        }
    }
}
