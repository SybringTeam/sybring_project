using Microsoft.AspNetCore.Mvc;
using sybring_project.Repos.Interfaces;

namespace sybring_project.ViewComponents
{
   
    public class HoildayViewComponent : ViewComponent
    {

        private readonly IHoildayService _hoildayService;

        public HoildayViewComponent(IHoildayService hoildayService)
        {
            _hoildayService = hoildayService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hoildayData = await _hoildayService.GetHoildayReport();
            return View("Index", hoildayData);
        }




    }
}
