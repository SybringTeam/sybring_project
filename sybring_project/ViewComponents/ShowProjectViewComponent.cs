using ActiveUp.Net.Security.OpenPGP.Packets;
using Microsoft.AspNetCore.Mvc;
using sybring_project.Repos.Interfaces;

namespace sybring_project.ViewComponents
{
    public class ShowProjectViewComponent:ViewComponent
    {

        private readonly IUserServices _userServices;
        private readonly IProjectServices _projectServices;
        public ShowProjectViewComponent(IUserServices userServices, IProjectServices projectService)
        {
            _userServices = userServices;
            _projectServices = projectService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            try
            {
                var project = await _projectServices.GetProjectByIdAsync(Id);

                if (project != null)
                {
                    ViewBag.AllProjects = project;
                }

                return View("Default", project);
            }
            catch (InvalidOperationException ex)
            {
               
                return View("ProjectNotFound");
            }

        }










    }
}
