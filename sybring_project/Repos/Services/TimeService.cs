﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using System.Threading.Tasks;

namespace sybring_project.Repos.Services
{
    public class TimeService : ITimeService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public TimeService(ApplicationDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<TimeHistory>> GetTimeListAsync(string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                // If the user is an admin, retrieve all billing data
                return await _db.TimeHistories
                    .Include(t => t.Users)
                    .ToListAsync();
            }
            else
            {
                // If the user is not an admin, retrieve only their own billing data
                return await _db.TimeHistories
                    .Include(t => t.Users)
                    .Where(b => b.Users.Any(u => u.Id == userId))
                    .ToListAsync();
            }



        }


        // Adding a new time history 
        public async Task AddTimeHistoryAsync(TimeHistory timeHistory)
        {
            _db.TimeHistories.Add(timeHistory);
            await _db.SaveChangesAsync();
        }


        //dawod work
        //public async Task AddReportAsync(DayDataVM dayDataVM, string userId, decimal scheduledHoursPerWeek)
        //{

        //    try
        //    {
        //        // Calculate working hours for the day (including overtime)
        //        decimal totalWorkingHours = CalculateWorkingHoursAsync(dayDataVM, scheduledHoursPerWeek);

        //        var timeReport = new TimeHistory
        //        {
        //            Date = dayDataVM.Date,
        //            //Schedule = GetPreviousMonday(dayData.Date),
        //            StartWork = dayDataVM.StartWork,
        //            EndWork = dayDataVM.EndWork,
        //            StartBreak = dayDataVM.StartBreak,
        //            EndBreak = dayDataVM.EndBreak,
        //            TotalWorkingHours = totalWorkingHours,
        //            //WorkingHours = dayDataVM.WorkingHours,
        //            WorkingHours = totalWorkingHours,
        //            FlexiTime = dayDataVM.FlexiTime,
        //            MoreTime = dayDataVM.MoreTime,
        //            AttendanceTime = dayDataVM.AttendanceTime,
        //            AnnualLeave = dayDataVM.AnnualLeave,
        //            SickLeave = dayDataVM.SickLeave,
        //            LeaveOfAbsence = dayDataVM.LeaveOfAbsence,
        //            Childcare = dayDataVM.Childcare,
        //            Overtime = dayDataVM.Overtime,
        //            InconvenientHours = dayDataVM.InconvenientHours,

        //        };

        //        var user = _db.Users.Find(userId);

        //        if (user != null)
        //        {
        //            timeReport.Users = new List<User> { user };// Assign the user to the Users collection

        //            _db.TimeHistories.Add(timeReport);
        //            _db.SaveChanges();
        //            //await _db.SaveChangesAsync();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // exception
        //        Console.WriteLine($"Error occurred while adding report: {ex.Message}");
        //        throw;
        //    }

        //}

        ////dawod work
        //        public decimal CalculateWorkingHoursAsync(DayDataVM dayDataVM, decimal scheduledHoursPerWeek)
        //{
        //    TimeSpan workDuration = dayDataVM.EndWork - dayDataVM.StartWork;
        //    TimeSpan breakDuration = dayDataVM.EndBreak - dayDataVM.StartBreak;

        //    // Calculate the total break duration in hours
        //    decimal totalBreakHours = (decimal)breakDuration.TotalHours;

        //    // Subtract the break duration from the total work duration
        //    decimal workingHours = (decimal)workDuration.TotalHours - totalBreakHours;

        //    if (workingHours > scheduledHoursPerWeek)
        //    {
        //        decimal excessHours = workingHours - scheduledHoursPerWeek;

        //        dayDataVM.Overtime = excessHours;

        //        workingHours = scheduledHoursPerWeek;
        //    }


        //    const decimal standardWorkingHoursPerDay = 8;
        //    if (workingHours > standardWorkingHoursPerDay)
        //    {
        //        // Calculate overtime
        //        decimal overtime = workingHours - standardWorkingHoursPerDay;

        //        // Limit to standard working hours
        //        workingHours = standardWorkingHoursPerDay;

        //        // Add overtime to total working hours
        //        workingHours += overtime;
        //    }

        //    return workingHours;
        //}



        //Spurti


public async Task AddReportAsync(DayDataVM dayDataVM, string userId, decimal scheduledHoursPerWeek)
        {                               
                decimal totalWorkingHours = CalculateWorkingHoursAsync(dayDataVM, scheduledHoursPerWeek);

                var timeReport = new TimeHistory
                {
                    Date = dayDataVM.Date,
                    StartWork = dayDataVM.StartWork,
                    EndWork = dayDataVM.EndWork,
                    StartBreak = dayDataVM.StartBreak,
                    EndBreak = dayDataVM.EndBreak,
                    TotalWorkingHours = totalWorkingHours,
                    WorkingHours = totalWorkingHours, 
                    FlexiTime = dayDataVM.FlexiTime,
                    MoreTime = dayDataVM.MoreTime,
                    AttendanceTime = dayDataVM.AttendanceTime,
                    AnnualLeave = dayDataVM.AnnualLeave,
                    SickLeave = dayDataVM.SickLeave,
                    LeaveOfAbsence = dayDataVM.LeaveOfAbsence,
                    Childcare = dayDataVM.Childcare,
                    Overtime = dayDataVM.Overtime,
                    InconvenientHours = dayDataVM.InconvenientHours,
                };

                var user = await _db.Users.FindAsync(userId);
                if (user != null)
                {
                    timeReport.Users = new List<User> { user }; 

                    _db.TimeHistories.Add(timeReport);
                    await _db.SaveChangesAsync();
                }
           }
	

        public decimal CalculateWorkingHoursAsync(DayDataVM dayDataVM, decimal scheduledHoursPerWeek)
        {
            
            TimeSpan workDuration = dayDataVM.EndWork - dayDataVM.StartWork;          
            TimeSpan breakDuration = dayDataVM.EndBreak - dayDataVM.StartBreak;
            decimal totalBreakHours = (decimal)breakDuration.TotalHours;

            
            decimal totalWorkHours = (decimal)workDuration.TotalHours;

           
            decimal workingHours = totalWorkHours - totalBreakHours;

           
            const decimal standardWorkingHoursPerDay = 8;

         
            decimal WorkingHours = Math.Min(workingHours, standardWorkingHoursPerDay);

           
            decimal overtime = 0;
            if (workingHours > standardWorkingHoursPerDay)
            {
                overtime = workingHours - standardWorkingHoursPerDay;
            }

            
            dayDataVM.Overtime = overtime;

            return WorkingHours;
        }



       



        // Helper method to get the previous Monday from a given date
        //public DateTime GetPreviousMonday(DateTime date)
        //{
        //    int daysUntilPrevMonday = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        //    return date.AddDays(-daysUntilPrevMonday).Date;
        //}



        //public async Task<List<decimal>> CalculateWeekDataAsync(TimeReportViewModel timeReportViewModel)
        //{

        //    var workingHoursList = new List<decimal>();
        //    var overtimeList = new List<decimal>();

        //    foreach (var dayData in timeReportViewModel.WeekData)
        //    {
        //        var workingHours = CalculateWorkingHoursAsync(dayData.StartWork, dayData.EndWork);
        //        workingHours -= CalculateWorkingHoursAsync(dayData.StartBreak, dayData.EndBreak);
        //        workingHoursList.Add(workingHours);

        //        // Calculate overtime
        //        var overtime = CalculateOvertime(workingHours, dayData.WorkingHours);
        //        overtimeList.Add(overtime);

        //        if (overtime <= 0) // If there is no overtime
        //        {
        //            workingHoursList.Add(workingHours);
        //        }
        //    }

        //    return workingHoursList;
        //}




        public async Task<TimeHistory> GetTimeHistoryByIdAsync(int id)
        {
            var time = await _db.TimeHistories
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (time == null)
            {
                throw new InvalidOperationException($"Time with ID {id} not found.");
            }
            return time;
        }




        // Deleting a time history 
        public async Task DeleteTimeHistoryAsync(int id)
        {
            var timeHistoryToDelete = await _db.TimeHistories.FindAsync(id);
            if (timeHistoryToDelete != null)
            {
                _db.TimeHistories.Remove(timeHistoryToDelete);
                await _db.SaveChangesAsync();
            }
        }


        // Updating an existing time history 
        public async Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory)
        {
            _db.Update(updatedTimeHistory);
            await _db.SaveChangesAsync();
        }


        ////  // Generate time report by days of the week
        ////  public async Task<Dictionary<string, double>> GenerateTimeReportByDaysAsync(DateTime startDate, DateTime endDate)
        ////  {
        ////      // Initialize dictionary to store total hours for each day of the week
        ////      var report = new Dictionary<string, double>
        ////{
        ////    { "Monday", 0 },
        ////    { "Tuesday", 0 },
        ////    { "Wednesday", 0 },
        ////    { "Thursday", 0 },
        ////    { "Friday", 0 }
        ////};

        ////      // Retrieve time entries within the specified date range
        ////      var timeEntries = await _context.TimeHistories
        ////          .Include(th => th.ProjectId)
        ////          .Where(th => th.DateTime >= startDate && th.DateTime <= endDate)
        ////          .ToListAsync();

        ////      // Aggregate time entries by day of the week
        ////      foreach (var timeEntry in timeEntries)
        ////      {
        ////          var dayOfWeek = timeEntry.DateTime.DayOfWeek.ToString();
        ////          report[dayOfWeek] += (timeEntry.DateTime - timeEntry.DateTime.Date).TotalHours; // Assuming time is recorded in hours
        ////      }

        ////      return report;
        ////  }


    }
}