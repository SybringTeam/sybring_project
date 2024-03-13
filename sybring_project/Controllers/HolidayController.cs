using Microsoft.AspNetCore.Mvc;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class HolidayController : Controller
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService hoildayService)
        {
            _holidayService = hoildayService;
        }

      

        public async Task<IActionResult> Index()
        {

            var hoildayData = await _holidayService.GetHolidayReportAsync();


            return View(hoildayData);
        }


        public async Task<IActionResult> MyIndex()
        {

            var holidayData = await _holidayService.GetHolidayDetails();


            return View(holidayData);
        }




    }
}
