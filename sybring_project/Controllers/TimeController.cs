using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class TimeController : Controller
    {
        private const decimal MaxRegularHoursPerDay = 8; // Declaration 

        private readonly ITimeService _timeService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TimeController(ApplicationDbContext context, 
            ITimeService timeService, UserManager<User> userManager)
        {

            _timeService = timeService;
            _context = context;
            _userManager = userManager;


        }

    //    public IActionResult Index()
    //    {
    //        // Retring time history  from the database
    //        var timeHistories = _context.TimeHistories.ToList(); 

    //        return View(timeHistories);
    //    }


    //    [HttpGet]
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

        
    //    [HttpPost]    
    //    public async Task<IActionResult> Create([Bind("DateTime")] TimeHistory timeHistory)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
                   
    //                await _timeService.AddTimeHistoryAsync(timeHistory);
    //                return RedirectToAction(nameof(Index));
    //            }
    //            catch (Exception)
    //            {
                   
    //                ModelState.AddModelError("", "An error occurred while saving the time history record.");
    //            }
    //        }
            
    //        return View(timeHistory);
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var timeHistory = await _context.TimeHistories.FirstOrDefaultAsync(m => m.Id == id);

    //        if (timeHistory == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(timeHistory);
    //    }



    //    [HttpGet]
    //    public IActionResult CreateReport()
    //    {
    //        var model = new TimeReportViewModel
    //        {
    //            WeekData = new List<TimeHistoryVM>
    //    {
    //        new TimeHistoryVM(),
    //        new TimeHistoryVM(),
    //        new TimeHistoryVM(),
    //        new TimeHistoryVM(),
    //        new TimeHistoryVM(),
    //        new TimeHistoryVM(),
    //        new TimeHistoryVM()
    //    }
    //        };

    //        return View(model);
    //    }



        

    //    [HttpPost]
    //    [Route("/Time/CreateReport")]
    //    public IActionResult CreateReport(TimeReportViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            try
    //            {
    //                // Calculate total work hours for each day
    //                foreach (var dayData in model.WeekData)
    //                {
    //                    // Validate input data
    //                    if (dayData.StartTime > dayData.EndTime)
    //                    {
    //                        ModelState.AddModelError("", "End time cannot be before start time.");
    //                        return View(model);
    //                    }



    //                    // Calculating total hours 
    //                    var totalHoursWithLunch = (decimal)(dayData.EndTime - dayData.StartTime).TotalHours;

    //                    // Subtracting lunch break duration
    //                    totalHoursWithLunch -= CalculateWorkingHours(dayData.LunchStart, dayData.LunchEnd);

                        
    //                    dayData.TotalWorkHours = totalHoursWithLunch;

    //                    // Calculating overtime hours 
    //                    dayData.OvertimeHours = dayData.TotalWorkHours > MaxRegularHoursPerDay ? dayData.TotalWorkHours - MaxRegularHoursPerDay : 0;
    //                }

    //                // Serializing and store the model data in TempData
    //                TempData["TimeReportModel"] = JsonConvert.SerializeObject(model);



    //                // Redirecting to the ReportDetails action
    //                return RedirectToAction("ReportDetails");
    //            }
    //            catch (Exception ex)
    //            {
    //                ModelState.AddModelError("", $"An error occurred while processing the time report: {ex.Message}");
    //            }
    //        }

    //        // If model state is not valid 
    //        return View(model);
    //    }



    //    // ReportDetails action
    //    public IActionResult ReportDetails()
    //    {
    //        // Retrieve the serialized model data from TempData
    //        var serializedModel = TempData["TimeReportModel"] as string;

    //        // Checking if serialized model data is null or empty
    //        if (string.IsNullOrEmpty(serializedModel))
    //        {
               

    //            return RedirectToAction("CreateReport");
    //        }

    //        // Deserialize the model data from JSON
    //        var timeReportModel = JsonConvert.DeserializeObject<TimeReportViewModel>(serializedModel);

    //        // Calculate and store only the working hours in a new list
    //        var workingHoursList = new List<decimal>();

    //        var overtimeList = new List<decimal>();

    //        // Initialing total work hours for the week
    //        decimal totalWorkHoursForWeek = 0;

    //        foreach (var dayData in timeReportModel.WeekData)
    //        {
    //            // Calculate working hours considering lunch break
    //            var workingHours = CalculateWorkingHours(dayData.StartTime, dayData.EndTime);
    //            workingHours -= CalculateWorkingHours(dayData.LunchStart, dayData.LunchEnd);

    //            // Add working hours to the list
    //            workingHoursList.Add(workingHours);

    //            // Calculate overtime (if applicable)
    //            var overtime = Math.Max(workingHours - MaxRegularHoursPerDay, 0);
    //            overtimeList.Add(overtime);

    //            // Add working hours to total work hours for the week
    //            totalWorkHoursForWeek += dayData.TotalWorkHours;

    //        }




    //        // Passing the working hours and overtime lists to the view
    //        ViewBag.WorkingHoursList = workingHoursList;
    //        ViewBag.OvertimeList = overtimeList;
         
    //        ViewBag.TotalWorkHoursForWeek = totalWorkHoursForWeek;


    //        return View();

    //    }


    //    // Method to calculate working hours
    //    private decimal CalculateWorkingHours(TimeSpan startTime, TimeSpan endTime)
    //{
    //    // Calculating working hours (total hours between start and end time)
    //    return (decimal)(endTime - startTime).TotalHours;
    //}







    }
}
