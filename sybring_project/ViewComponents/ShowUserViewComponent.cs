using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;

namespace sybring_project.ViewComponents
{
    public class ShowUserViewComponent:ViewComponent
    {

        private readonly IUserServices _userServices;

        public ShowUserViewComponent(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public IViewComponentResult Invoke(string userId)
        {
            var user = _userServices.GetUserById(userId);
            return View("Default", user);
        }

    }
}
