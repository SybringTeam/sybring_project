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



        public TimeController(ApplicationDbContext context,
            ITimeService timeService, UserManager<User> userManager,
            IUserServices userServices, IHolidayService holidayService)
        {

            _timeService = timeService;
            _context = context;
            _userManager = userManager;
            _userServices = userServices;
            _holidayService = holidayService;



        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var timeHistories = await _timeService.GetTimeListAsync(userId);

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

            var viewModel = new TimeHistoryViewModel
            {
                TimeHistories = timeHistories,
                UserList = new SelectList(userList, "Value", "Text"),
                DateRanges = new SelectList(dateRanges, "Value", "Text")
            };

            return View(viewModel);
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
                    StartWork = TimeSpan.FromHours(8), // Set default values for StartWork, EndWork, etc.
                    EndWork = TimeSpan.FromHours(17),
                    StartBreak = TimeSpan.FromHours(12),
                    EndBreak = TimeSpan.FromHours(13),

                };
                model.Add(dayData);
            }

            return View(model);
        }




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







    }
}
