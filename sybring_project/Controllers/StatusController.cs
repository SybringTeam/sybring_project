using Microsoft.AspNetCore.Mvc;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using System.Composition.Convention;

namespace sybring_project.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusService _statusService;
        private readonly IUserServices _userServices;

        public StatusController(IStatusService statusService, IUserServices userServices)
        {
            _statusService = statusService;
            _userServices = userServices;
        }
        public async Task<IActionResult> Index()
        {
            var statusList = await _statusService.GetStatusListAsync();
            return View(statusList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Status status)
        {
            await _statusService.AddStatusAsync(status);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _statusService.DeleteStatusAsync(id);
            return RedirectToAction("Index");

        }

    }
}
