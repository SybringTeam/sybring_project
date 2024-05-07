using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models;
using sybring_project.Models.Db;
using sybring_project.Models.Seeding;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using sybring_project.Repos.Services;
using System.Globalization;
using System.Security.Claims;


namespace sybring_project.Controllers
{
    public class TimeController : Controller
    {
        //private const decimal MaxRegularHoursPerDay = 8; // Declaration 

        private const decimal MaxRegularHoursPerDay = 8; // Declaration 

        private readonly ITimeService _timeService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserServices _userServices;
        private readonly IHolidayService _holidayService;
        private readonly ApplicationDbContext _db;


        public TimeController(ApplicationDbContext context,
            ITimeService timeService, UserManager<User> userManager,
            IUserServices userServices, IHolidayService holidayService, ApplicationDbContext db)
        {

            _timeService = timeService;
            _context = context;
            _userManager = userManager;
            _userServices = userServices;
            _holidayService = holidayService;
            _db = db;



        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var timeHistories = await _timeService.GetTimeListAsync(userId);

            // Group time histories by month
            var timeHistoriesByMonth = timeHistories.GroupBy(th => th.Date.ToString("yyyy-MM"))
                                                     .ToDictionary(g => g.Key, g => g.ToList());

            var userList = await _userManager.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = $"{u.FirstName} {u.LastName}"
            }).ToListAsync();

            var dateRanges = new List<SelectListItem>
    {
        new SelectListItem { Value = "week", Text = "Week" },
        new SelectListItem { Value = "month", Text = "Month" },
        new SelectListItem { Value = "day", Text = "Day" }
    };

            var currentUser = $"{User.Identity.Name}";

            var viewModel = new TimeHistoryViewModel
            {
                TimeHistoriesByMonth = timeHistoriesByMonth, // Pass grouped data to the view model
                UserList = new SelectList(userList, "Value", "Text"),
                DateRanges = new SelectList(dateRanges, "Value", "Text"),
                CurrentUser = currentUser
            };

            return View(viewModel);
        }







        public async Task<IActionResult> Details(int? id, string month)
        {
            if (id != null)
            {
                // Retrieve details for a specific time history based on id
                var timeHistory = await _timeService.GetTimeHistoryByIdAsync(id.Value);
                if (timeHistory == null)
                {
                    return NotFound();
                }
                return View(timeHistory);
            }
            else if (!string.IsNullOrEmpty(month))
            {
                // Retrieve a list of time histories for the specified month
                var timeHistories = await _timeService.GetTimeHistoriesForMonthAsync(month);
                return View(timeHistories); // Render the "Details" view with the list of time histories
            }
            else
            {
                return BadRequest("Invalid parameters provided.");
            }
        }




        [Authorize(Roles = "admin, superadmin, underconsult")]
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
                  
                    // Set default values for StartWork, EndWork, StartBreak, and EndBreak
                    StartWork = IsWeekend(currentDateInLoop) ? TimeSpan.Zero : TimeSpan.FromHours(8),
                    EndWork = IsWeekend(currentDateInLoop) ? TimeSpan.Zero : TimeSpan.FromHours(17),
                    StartBreak = IsWeekend(currentDateInLoop) ? TimeSpan.Zero : TimeSpan.FromHours(12),
                    EndBreak = IsWeekend(currentDateInLoop) ? TimeSpan.Zero : TimeSpan.FromHours(13),

                };
                model.Add(dayData);
            }

          
            return View(model);
        }

        // Function to check if the given date is a weekend (Saturday or Sunday)
        public bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }



        //good work without validation

        //[Authorize(Roles = "Admin,underconsult")]
        //[HttpPost]
        //public async Task<IActionResult> Create(List<DayDataVM> weekData, decimal scheduledHoursPerWeek)
        //{
        //    if (weekData == null || weekData.Count == 0)
        //    {
        //        return BadRequest("No data provided.");
        //    }

        //    var userId = _userManager.GetUserId(User);

        //    foreach (var dayData in weekData)
        //    {

        //        // Add the report
        //        await _timeService.AddReportAsync(dayData, userId, scheduledHoursPerWeek);

        //    }

        //    return RedirectToAction("Index");
        //}




        //recent work for validation

        [HttpPost]
        [Authorize(Roles = "Admin, underconsult")]
        public async Task<IActionResult> Create(List<DayDataVM> weekData, decimal scheduledHoursPerWeek)
        {
            if (weekData == null || weekData.Count == 0)
            {
                return BadRequest("No data provided.");
            }

            var userId = _userManager.GetUserId(User);

            // Fetching existing time reports for the user
            var existingTimeReportsForUser = await _db.TimeHistories
                .Include(t => t.Users)
                .Where(t => t.Users.Any(u => u.Id == userId))
                .ToListAsync();

            // Checking if any time reports already exist
            var existingTimeReports = existingTimeReportsForUser
                .Where(t => weekData.Any(d => d.Date.Date == t.Date.Date))
                .ToList();

            if (existingTimeReports.Count > 0)
            {
                ModelState.AddModelError("", "A time report already exists. You cannot submit a new report for the selected week.");
                return View("Create", weekData); // Return the view with validation errors
            }

            foreach (var dayData in weekData)
            {
                // Adding the report to database
                await _timeService.AddReportAsync(dayData, userId, scheduledHoursPerWeek);
            }

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int id)
        {
            await _timeService.DeleteTimeHistoryAsync(id);
            return RedirectToAction("Index");

        }







        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var getById = _timeService.GetTimeHistoryByIdAsync(id);
            return View(getById);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TimeHistory updatedTimeHistory)
        {
            var edit = _timeService.UpdateTimeHistoryAsync(updatedTimeHistory);
            return View(edit);
        }




        //fetching red days and weeknumbers of year from api
        public async Task<IActionResult> RedDays()
        {


            // Retrieve all red days for the year 2024
            var redDays = await _holidayService.GetRedDaysAsync();

            // Pass the red days to the view
            return View(redDays);
        }


        public IActionResult projectVc(int Id)
        {

            return ViewComponent("ShowProject", new { projectId = Id });


        }

        [HttpPost]
        public async Task<IActionResult> SelectUserAndDate(string selectedUserId, string selectedDateRange)
        {
            var timeHistories = await _timeService.GetTimeHistoriesAsync(selectedUserId, selectedDateRange);

            var users = await _userManager.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = $"{u.FirstName} {u.LastName}"
            }).ToListAsync();

            var dateRanges = new List<SelectListItem>
            {
                new SelectListItem { Value = "week", Text = "Week" },
                new SelectListItem { Value = "month", Text = "Month" },
                new SelectListItem { Value = "day", Text = "Day" }
            };

            var model = new TimeHistoryViewModel
            {
                UserList = new SelectList(users, "Value", "Text"),
                DateRanges = new SelectList(dateRanges, "Value", "Text"),
                TimeHistories = timeHistories
            };

            return View("Index", model);
        }



        [HttpGet]
        public async Task<IActionResult> GetTimeHistoriesByWeek(string userId)
        {
            var timeHistories = await _timeService.GetHistoryByWeekNUser(userId, "week");
            return Ok(timeHistories);
        }




    }
}
