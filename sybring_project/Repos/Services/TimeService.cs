using Microsoft.AspNetCore.Identity;
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
	

        //Spurti

        //public decimal CalculateWorkingHoursAsync(DayDataVM dayDataVM, decimal scheduledHoursPerWeek)
        //{

        //    TimeSpan workDuration = dayDataVM.EndWork - dayDataVM.StartWork;          
        //    TimeSpan breakDuration = dayDataVM.EndBreak - dayDataVM.StartBreak;
        //    decimal totalBreakHours = (decimal)breakDuration.TotalHours;


        //    decimal totalWorkHours = (decimal)workDuration.TotalHours;


        //    decimal workingHours = totalWorkHours - totalBreakHours;


        //    const decimal standardWorkingHoursPerDay = 8;


        //    decimal WorkingHours = Math.Min(workingHours, standardWorkingHoursPerDay);


        //    decimal overtime = 0;
        //    if (workingHours > standardWorkingHoursPerDay)
        //    {
        //        overtime = workingHours - standardWorkingHoursPerDay;
        //    }


        //    dayDataVM.Overtime = overtime;

        //    return WorkingHours;
        //}






        //Spurti
        public decimal CalculateWorkingHoursAsync(DayDataVM dayDataVM, decimal scheduledHoursPerWeek)
        {
            // Checking if StartWork, EndWork, StartBreak, and EndBreak are null or have default values
            if (dayDataVM.StartWork == TimeSpan.Zero &&
                dayDataVM.EndWork == TimeSpan.Zero &&
                dayDataVM.StartBreak == TimeSpan.Zero &&
                dayDataVM.EndBreak == TimeSpan.Zero)
            {
                // Calculate working hours based on TotalWorkingHours input by user
                decimal totalWorkHours = dayDataVM.TotalWorkingHours;
                const decimal standardWorkingHoursPerDay = 8;
                decimal workingHours = Math.Min(totalWorkHours, standardWorkingHoursPerDay);

                decimal overtime = 0;
                if (totalWorkHours > standardWorkingHoursPerDay)
                {
                    overtime = totalWorkHours - standardWorkingHoursPerDay;

                }

                dayDataVM.Overtime = overtime;
                

                return workingHours;
            }
            else
            {
                // Calculate working hours 
                TimeSpan workDuration = dayDataVM.EndWork - dayDataVM.StartWork;
                TimeSpan lunchBreak = TimeSpan.FromHours(1); 
                TimeSpan totalWorkDuration = workDuration - lunchBreak; 
                decimal totalWorkHours = (decimal)totalWorkDuration.TotalHours;

                const decimal standardWorkingHoursPerDay = 8;
                decimal workingHours = Math.Min(totalWorkHours, standardWorkingHoursPerDay);

                decimal overtime = 0;
                if (totalWorkHours > standardWorkingHoursPerDay)
                {
                    overtime = totalWorkHours - standardWorkingHoursPerDay;
                }

                dayDataVM.Overtime = overtime;

                return workingHours;
            }
        }





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


        public async Task<IEnumerable<TimeHistory>> GetTimeHistoriesAsync(string userId, string dateRange)
        {
            DateTime startDate;
            DateTime endDate = DateTime.Now;

            switch (dateRange.ToLower())
            {
                case "week":
                    startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
                    endDate = startDate.AddDays(6); // Sista dagen i veckan
                    break;
                case "month":
                    startDate = new DateTime(endDate.Year, endDate.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1); // Sista dagen i månaden
                    break;
                case "day":
                    startDate = endDate.Date;
                    endDate = startDate.AddDays(1).AddTicks(-1); // Sista sekunden av dagen
                    break;
                default:
                    throw new ArgumentException("Invalid date range");
            }

            var currentUser = await _userManager.FindByIdAsync(userId);

            IQueryable<TimeHistory> query = _db.TimeHistories
                .Include(t => t.Users)
                .Where(t => t.Users.Any(u => u.Id == userId) && t.Date >= startDate && t.Date <= endDate);

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                query = query.Where(t => t.Users.Any(u => u.Id == userId));
            }

            return await query.ToListAsync();
        }



    }
}