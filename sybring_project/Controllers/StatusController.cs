using Microsoft.AspNetCore.Mvc;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }
        public async Task<IActionResult> Index()
        {
            var statusList = await _statusService.GetStatusListAsync();
            return View(statusList);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _statusService.DeleteStatusAsync(id);
            return RedirectToAction("Index");

        }

    }
}
