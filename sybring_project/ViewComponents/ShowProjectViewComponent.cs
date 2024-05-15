using ActiveUp.Net.Security.OpenPGP.Packets;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using sybring_project.Models.Db;
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

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            try
            {
                var project = await _projectServices.GetProjectByIdAsync(projectId);

                if (project != null)
                {

                    var assignedUsers = await _projectServices.GetAssignedUserForProjectAsync(projectId);

                    ViewBag.Project = project;

                    ViewBag.AssignedUsers = assignedUsers;

                    if (assignedUsers != null)
                    {
                        ViewBag.AssignedUsers = assignedUsers;
                    }
                    else
                    {

                        ViewBag.AssignedUsers = new List<User>();
                    }

                }


                return View("Default", project);
            }
            catch (Exception )
            {

                return View("Component Not Found");

            }

        }

    }

}

