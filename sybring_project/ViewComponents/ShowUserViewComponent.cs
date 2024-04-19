using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;

namespace sybring_project.ViewComponents
{
    public class ShowUserViewComponent:ViewComponent
    {

        private readonly IUserServices _userServices;
        private readonly IProjectServices _projectServices;
        private readonly ICompanyServices _companyServices;

        public ShowUserViewComponent(IUserServices userServices, IProjectServices projectService, ICompanyServices companyServices)
        {
            _userServices = userServices;
            _projectServices = projectService;   
            _companyServices = companyServices;
        }

        //public IViewComponentResult Invoke(string userId)
        //{
        //    var user = _userServices.GetUserById(userId);
        //    return View("Default", user);
        //} 

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            try
            {
                
                var user = await _userServices.GetUserByIdAsync(userId);
                ViewBag.User = user;
                
                var allProjects = await _projectServices.GetProjectsAsync();

                if (allProjects != null)
                {
                    ViewBag.AllProjects = allProjects;
                   
                }

                return View("Default", user);
            }
            catch (InvalidOperationException ex)
            {
                return View("UserNotFound");
            }
        }
      
    }
}
