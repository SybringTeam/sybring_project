using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;

namespace sybring_project.ViewComponents
{
    public class ShowUserViewComponent:ViewComponent
    {

        private readonly UserServices _userServices;

        public ShowUserViewComponent(UserServices userServices)
        {
            _userServices = userServices;
        }

        public IViewComponentResult Invoke(string userId)
        {
            var user = _userServices.GetUserByIdAsync(userId);
            return View("Index", user);
        }

    }
}
