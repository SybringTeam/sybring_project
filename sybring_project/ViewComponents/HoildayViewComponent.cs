using Microsoft.AspNetCore.Mvc;
using sybring_project.Repos.Interfaces;

namespace sybring_project.ViewComponents
{
   
    public class HolidayViewComponent : ViewComponent
    {

        private readonly IHolidayService _holidayService;

        public HolidayViewComponent(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var holidayData = await _holidayService.GetHolidayReportAsync();
            return View("Index", holidayData);
        }




    }
}
