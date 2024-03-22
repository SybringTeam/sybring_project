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

        public ShowUserViewComponent(IUserServices userServices, IProjectServices projectService)
        {
            _userServices = userServices;
            _projectServices = projectService;   
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
                // Handle the case where the user is not found
                // For example, you could return a view indicating that the user was not found
                return View("UserNotFound");
            }
        }
      
    }
}
