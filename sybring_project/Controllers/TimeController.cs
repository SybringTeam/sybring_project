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
using sybring_project.Repos.Services;
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
        private readonly IHolidayService _holidayService;


        public TimeController(ApplicationDbContext context,
            ITimeService timeService, UserManager<User> userManager,
            IUserServices userServices , IHolidayService holidayService)
        {

            _timeService = timeService;
            _context = context;
            _userManager = userManager;
            _userServices = userServices;
            _holidayService = holidayService;


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

        //dowad work


        //[Authorize(Roles = "Admin, underconsult")]
        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    List<DayDataVM> model = new List<DayDataVM>();

        //    // Calculate the start date of the current week (Monday)
        //    DateTime currentDate = DateTime.Today;
        //    DateTime startDate = currentDate;

        //    while (startDate.DayOfWeek != DayOfWeek.Monday)
        //    {
        //        startDate = startDate.AddDays(-1);
        //    }

        //    // Generate data for the week
        //    for (int i = 0; i < 7; i++)
        //    {
        //        DateTime currentDateInLoop = startDate.AddDays(i);
        //        var dayData = new DayDataVM
        //        {
        //            Date = currentDateInLoop,
        //            StartWork = TimeSpan.FromHours(8), // Set default values for StartWork, EndWork, etc.
        //            EndWork = TimeSpan.FromHours(17),
        //            StartBreak = TimeSpan.FromHours(12),
        //            EndBreak = TimeSpan.FromHours(13)
        //        };
        //        model.Add(dayData);
        //    }

        //    return View(model);
        //}


        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    // Fetching week data from the API using IHolidayService
        //    var holidayReport = await _holidayService.GetHolidayReportAsync();

        //    // Create a DayDataVM object using the day of the week obtained from the holiday report
        //    var dayOfWeek = holidayReport.Datum.DayOfWeek;
        //    var dayData = new DayDataVM
        //    {
        //        Date = holidayReport.Datum,

        //    };

        //    // Create a list to hold the single DayDataVM object
        //    var model = new List<DayDataVM> { dayData };

        //    return View(model);
        //}


        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    // Fetching week data from the API using IHolidayService
        //    var holidayReport = await _holidayService.GetHolidayReportAsync();

        //    // Initialize a list to hold DayDataVM objects for each day of the week
        //    var model = new List<DayDataVM>();

        //    // Assuming holidayReport.Datum represents the start of the week
        //    var startDate = holidayReport.Datum.Date;

        //    // Loop through each day of the week
        //    for (int i = 0; i < 7; i++)
        //    {
        //        // Create a DayDataVM object for the current day
        //        var dayData = new DayDataVM
        //        {
        //            Date = startDate.AddDays(i), // Incrementing the date for each day of the week
        //                                         // Add other properties as needed
        //        };

        //        // Add the DayDataVM object to the list
        //        model.Add(dayData);
        //    }

        //    return View(model);
        //}











        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    List<DayDataVM> model = new List<DayDataVM>();

        //    // Fetch week data from the API using IHolidayService
        //    var holidayReport = await _holidayService.GetHolidayReportAsync();

        //    // Ensure holidayReport is treated as a collection or enumerable
        //    if (holidayReport is IEnumerable<Holiday>)
        //    {
        //        // Iterate over the weekdays in the holiday report and create DayDataVM objects
        //        foreach (var holiday in (IEnumerable<Holiday>)holidayReport)
        //        {
        //            var dayData = new DayDataVM
        //            {
        //                Date = holiday.Datum,
        //                StartWork = TimeSpan.FromHours(8), // Set default values for StartWork, EndWork, etc.
        //                EndWork = TimeSpan.FromHours(17),
        //                StartBreak = TimeSpan.FromHours(12),
        //                EndBreak = TimeSpan.FromHours(13)
        //            };

        //            // Check if it's a red day and mark the day as such
        //            if (holiday.Röddag.ToLower() == "ja")
        //            {
        //                dayData.IsRedDay = true;
        //            }

        //            model.Add(dayData);
        //        }
        //    }
        //    else
        //    {
        //        // Handle the case where holidayReport is not a collection or enumerable
        //        // Log or handle the error accordingly
        //    }

        //    return View(model);
        //}


        














        //dowad work
        //[Authorize(Roles = "Admin,underconsult")]
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


        public async Task<IActionResult> RedDays()
        {

            //// Retrieve all red days for the current year
            //var currentYear = DateTime.Now.Year;

            // Retrieve all red days for the year 2024
            var redDays = await _holidayService.GetRedDaysAsync();

            // Pass the red days to the view
            return View(redDays);
        }


       









    }
}
