﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public TimeController(ApplicationDbContext context, ITimeService timeService, UserManager<User> userManager)
        {

            _timeService = timeService;
            _context = context;
            _userManager = userManager;


        }

        public async Task<IActionResult> Index()
        {
            var list = await _timeService.GetTimeListAsync();

            return View(list);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(TimeHistory timeHistory)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    await _timeService.AddTimeHistoryAsync(timeHistory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "An error occurred while saving the time history record.");
                }
            }

            return View(timeHistory);
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



        [HttpGet]
        public async Task<IActionResult> CreateReport()
        {
            TimeReportViewModel timeReportViewModel = new TimeReportViewModel();
            return View(timeReportViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport(TimeReportViewModel model)
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







        // ReportDetails action
        //public IActionResult ReportDetails()
        //{
        //    // Retrieve the serialized model data from TempData
        //    var serializedModel = TempData["TimeReportModel"] as string;

        //    // Checking if serialized model data is null or empty
        //    if (string.IsNullOrEmpty(serializedModel))
        //    {


        //        return RedirectToAction("CreateReport");
        //    }

        //    // Deserialize the model data from JSON
        //    var timeReportModel = JsonConvert.DeserializeObject<TimeReportViewModel>(serializedModel);

        //    // Calculate and store only the working hours in a new list
        //    var workingHoursList = new List<decimal>();

        //    var overtimeList = new List<decimal>();

        //    // Initialing total work hours for the week
        //    decimal totalWorkHoursForWeek = 0;

        //    foreach (var dayData in timeReportModel.WeekData)
        //    {
        //        // Calculate working hours considering lunch break
        //        var workingHours = CalculateWorkingHoursAsync(dayData.StartTime, dayData.EndTime);
        //        workingHours -= CalculateWorkingHours(dayData.LunchStart, dayData.LunchEnd);

        //        // Add working hours to the list
        //        workingHoursList.Add(workingHours);

        //        // Calculate overtime (if applicable)
        //        var overtime = Math.Max(workingHours - MaxRegularHoursPerDay, 0);
        //        overtimeList.Add(overtime);

        //        // Add working hours to total work hours for the week
        //        totalWorkHoursForWeek += dayData.TotalWorkHours;

        //    }




        //    // Passing the working hours and overtime lists to the view
        //    ViewBag.WorkingHoursList = workingHoursList;
        //    ViewBag.OvertimeList = overtimeList;

        //    ViewBag.TotalWorkHoursForWeek = totalWorkHoursForWeek;


        //    return View();

        //}


        //// Method to calculate working hours
        //private decimal CalculateWorkingHours(TimeSpan startTime, TimeSpan endTime)
        //{
        //    // Calculating working hours (total hours between start and end time)
        //    return (decimal)(endTime - startTime).TotalHours;
        //}







    }
}