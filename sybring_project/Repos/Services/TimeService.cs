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

        public TimeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TimeHistory>> GetTimeListAsync()
        {
            return await _db.TimeHistories.ToListAsync();
        }


        // Adding a new time history 
        public async Task AddTimeHistoryAsync(TimeHistory timeHistory)
        {
            _db.TimeHistories.Add(timeHistory);
            await _db.SaveChangesAsync();
        }


        public async Task AddReportAsync(DayDataVM dayDataVM)
        {
            var timeReport = new TimeHistory
            {
                Date = dayDataVM.Date,
                //Schedule = GetPreviousMonday(dayData.Date),
                StartWork = dayDataVM.StartWork,
                EndWork = dayDataVM.EndWork,
                StartBreak = dayDataVM.StartBreak,
                EndBreak = dayDataVM.EndBreak,
                TotalWorkingHours = dayDataVM.TotalWorkingHours,
                WorkingHours = dayDataVM.WorkingHours,
                FlexiTime = dayDataVM.FlexiTime,
                MoreTime = dayDataVM.MoreTime,
                AttendanceTime = dayDataVM.AttendanceTime,
                AnnualLeave = dayDataVM.AnnualLeave,
                SickLeave = dayDataVM.SickLeave,
                LeaveOfAbsence = dayDataVM.LeaveOfAbsence,
                Childcare = dayDataVM.Childcare,
                Overtime = dayDataVM.Overtime,
                InconvenientHours = dayDataVM.InconvenientHours,
                //ProjectHistories = dayData.ProjectHistories,
                //Users = _db.Users.Include(u => u.UserName).ToList()

            };

            _db.TimeHistories.Add(timeReport);


            _db.SaveChanges();

        }


        // Helper method to get the previous Monday from a given date
        public DateTime GetPreviousMonday(DateTime date)
        {
            int daysUntilPrevMonday = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            return date.AddDays(-daysUntilPrevMonday).Date;
        }




        public async Task<List<decimal>> CalculateWeekDataAsync(TimeReportViewModel timeReportViewModel)
        {

            var workingHoursList = new List<decimal>();
            var overtimeList = new List<decimal>();

            foreach (var dayData in timeReportViewModel.WeekData)
            {
                var workingHours = CalculateWorkingHoursAsync(dayData.StartWork, dayData.EndWork);
                workingHours -= CalculateWorkingHoursAsync(dayData.StartBreak, dayData.EndBreak);
                workingHoursList.Add(workingHours);

                // Calculate overtime
                var overtime = CalculateOvertime(workingHours, dayData.WorkingHours);
                overtimeList.Add(overtime);

                if (overtime <= 0) // If there is no overtime
                {
                    workingHoursList.Add(workingHours);
                }
            }

            return workingHoursList;
        }


        public decimal CalculateOvertime(decimal workingHours, decimal maxRegularHoursPerDay)
        {
            return Math.Max(workingHours - maxRegularHoursPerDay, 0);
        }

        public decimal CalculateWorkingHoursAsync(TimeSpan startTime, TimeSpan endTime)
        {
            // Calculating working hours (total hours between start and end time)
            return (decimal)(endTime - startTime).TotalHours;
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

        public async Task AssigUserToTimeAsync(string userId, int timeId)
        {
            var existingUser = await _db.Users
                .Include(u => u.TimeId)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var timeToAdd = await _db.TimeHistories
                .FirstOrDefaultAsync(t => t.Id == timeId);

            if (existingUser != null && timeToAdd != null)
            {
                existingUser.TimeId.Add(timeToAdd);
                await _db.SaveChangesAsync();
            }
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