using Microsoft.AspNetCore.Mvc;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class HoildayController : Controller
    {
        private readonly IHoildayService _hoildayService;

        public HoildayController(IHoildayService hoildayService)
        {
            _hoildayService = hoildayService;
        }
        //public IActionResult Index()
        //{
        //    return ViewComponent("HoildayViewComponent");
        //}

        public async Task<IActionResult> Index()
        {
            
            var hoildayData = await _hoildayService.GetHoildayReport();

            
            return View(hoildayData);
        }





    }
}
