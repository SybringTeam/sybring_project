using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.Seeding;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using System.Security.Claims;


namespace sybring_project.Controllers
{
    public class TimeController : Controller
    {
        private const decimal MaxRegularHoursPerDay = 8; // Declaration 

        private readonly ITimeService _timeService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserServices _userServices;


        public TimeController(ApplicationDbContext context,
            ITimeService timeService, UserManager<User> userManager,
            IUserServices userServices)
        {

            _timeService = timeService;
            _context = context;
            _userManager = userManager;
            _userServices = userServices;


        }

        public async Task<IActionResult> Index()
        {
            var list = await _timeService.GetTimeListAsync();

            return View(list);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeHistory = await _context.TimeHistories.FirstOrDefaultAsync(m => m.Id == id);

            if (timeHistory == null)
            {
                return NotFound();
            }

            return View(timeHistory);
        }


        [Authorize(Roles = "Admin, underconsult")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<DayDataVM> model = new List<DayDataVM>();

            // Calculate the start date of the current week (Monday)
            DateTime currentDate = DateTime.Today;
            DateTime startDate = currentDate;

            while (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(-1);
            }

            // Generate data for the week
            for (int i = 0; i < 7; i++)
            {
                DateTime currentDateInLoop = startDate.AddDays(i);
                var dayData = new DayDataVM
                {
                    Date = currentDateInLoop,
                    StartWork = TimeSpan.FromHours(8), // Set default values for StartWork, EndWork, etc.
                    EndWork = TimeSpan.FromHours(17),
                    StartBreak = TimeSpan.FromHours(12),
                    EndBreak = TimeSpan.FromHours(13)
                };
                model.Add(dayData);
            }

            return View(model);
        }


        [Authorize(Roles = "Admin,underconsult")]
        [HttpPost]
        public async Task<IActionResult> Create(List<DayDataVM> weekData, decimal scheduledHoursPerWeek)
        {
            if (weekData == null || weekData.Count == 0)
            {
                return BadRequest("No data provided.");
            }
            var userId = _userManager.GetUserId(User);

            foreach (var dayData in weekData)
            {
               
                // Add the report
                await _timeService.AddReportAsync(dayData, userId, scheduledHoursPerWeek);
               
            }
           
            return RedirectToAction("Index");
        }


        ////ReportDetails action
        //public async Task<IActionResult> ReportDetails(DayDataVM dayData)
        //{
        //    try
        //    {
        //        // Calculate week data including working hours and overtime
        //        var workingHoursList = await _timeService.CalculateWeekDataAsync(dayData);
        //        var overtimeList = await _timeService.CalculateWeekDataAsync(dayData);

        //        // Check if any overtime hours exist
        //        var hasOvertime = overtimeList.Any(overtime => overtime > 0);

        //        // Pass the appropriate data to the view
        //        if (hasOvertime)
        //        {
        //            ViewBag.OvertimeList = overtimeList;
        //        }
        //        else
        //        {
        //            ViewBag.WorkingHoursList = workingHoursList;
        //        }

        //        return View(timeReportViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = ex.Message;
        //        return View("Error");
        //    }

        //}






        ////second version




        ////[HttpGet]
        //public IActionResult CreateReportTextfield()
        //{
        //    var model = new TimeReportViewModel();


        //    WeekData = new List<DayData>
        //        {
        //            new DayData(),
        //            new DayData(),
        //            new DayData(),
        //            new DayData(),
        //            new DayData(),
        //            new DayData(),
        //            new DayData()
        //        };


        //    return View(model);
        //}

        //[HttpPost]
        //[Route("/Time/CreateReportTextfield")]
        //public IActionResult CreateReportTextfield(TimeReportViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {

        //        return View(model);
        //    }

        //    try
        //    {

        //        decimal totalWeekWorkHours = 0;


        //        foreach (var dayData in model.WeekData)
        //        {

        //            if (dayData.StartTime > dayData.EndTime)
        //            {
        //                ModelState.AddModelError("", "End time cannot be before start time.");
        //                return View(model);
        //            }

        //            var totalHoursWithLunch = (decimal)(dayData.EndTime - dayData.StartTime).TotalHours;

        //            totalHoursWithLunch -= CalculateWorkingHours(dayData.LunchStart, dayData.LunchEnd);

        //            dayData.TotalWorkHours = totalHoursWithLunch;


        //            totalWeekWorkHours += dayData.TotalWorkHours;


        //        }

        //        model.TotalWorkHours = totalWeekWorkHours;


        //        // Serialize and store the model data in TempData
        //        TempData["TimeReportModel"] = JsonConvert.SerializeObject(model);


        //        return View();
        //    }
        //    catch (Exception ex)
        //    {

        //        ModelState.AddModelError("", $"An error occurred while processing the time report: {ex.Message}");
        //        return View(model);
        //    }
        //}




    }
}
