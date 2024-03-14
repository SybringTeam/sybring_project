using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
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
        public async Task<IActionResult> TimeToUser(int id)
        {
            var time = await _timeService.GetTimeHistoryByIdAsync(id);

            if (time.ProjectHistories == null || !time.ProjectHistories.Any())
            {
                ViewBag.NoTimeHistoryMessage = "New user has no time to show.";
            }

            var allTime = await _context.Users.ToListAsync();

            if (allTime != null)
            {
                ViewBag.AllTime = allTime;
            }

            return View(time);
        }

        [HttpPost]
        public async Task<IActionResult> TimeToUser(string userId, int timeId)
        {
            try
            {
                var getTime = await _timeService.GetTimeHistoryByIdAsync(timeId);
                var getUser = await _userServices.GetUserByIdAsync(userId);

                if (getTime == null || getUser == null)
                {
                    return NotFound("User or TimeHistory Not Found");
                }

                await _timeService.AssigUserToTimeAsync(userId, timeId);
                TempData["Added"] = "This User has been assigned to the time.";
                return RedirectToAction("Details", new { id = timeId });
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }
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
            TimeReportViewModel timeReportViewModel = new TimeReportViewModel();
            return View(timeReportViewModel);
        }

        [Authorize(Roles = "Admin,underconsult")]
        [HttpPost]
        public async Task<IActionResult> Create(TimeReportViewModel model)
        {
            try
            {
                await _timeService.AddReportAsync(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while processing the time report: {ex.Message}");
                return View(model);
            }
        }


        ////ReportDetails action
        public async Task<IActionResult> ReportDetails(TimeReportViewModel timeReportViewModel)
        {
            try
            {
                // Calculate week data including working hours and overtime
                var workingHoursList = await _timeService.CalculateWeekDataAsync(timeReportViewModel);
                var overtimeList = await _timeService.CalculateWeekDataAsync(timeReportViewModel);

                // Check if any overtime hours exist
                var hasOvertime = overtimeList.Any(overtime => overtime > 0);

                // Pass the appropriate data to the view
                if (hasOvertime)
                {
                    ViewBag.OvertimeList = overtimeList;
                }
                else
                {
                    ViewBag.WorkingHoursList = workingHoursList;
                }

                return View(timeReportViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }

    
     



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
