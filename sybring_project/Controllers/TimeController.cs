using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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






        //fetch red days and weeknumbers of year from api
        public async Task<IActionResult> RedDays()
        {

            //// Retrieve all red days for the current year
            //var currentYear = DateTime.Now.Year;

            // Retrieve all red days for the year 2024
            var redDays = await _holidayService.GetRedDaysAsync();

            // Pass the red days to the view
            return View(redDays);
        }


       
        public IActionResult projectVc(int Id)
        {

            return ViewComponent("ShowProject", new { projectId = Id });


        }



    }
}
