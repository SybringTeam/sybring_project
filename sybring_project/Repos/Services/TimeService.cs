using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class TimeService : ITimeService
    {
        private readonly ApplicationDbContext _db;

        public TimeService(ApplicationDbContext db)
        {
            _db = db;
        }



        // Adding a new time history 
        public async Task AddTimeHistoryAsync(TimeHistoryVM timeHistoryVM, string userId)
        {
            TimeHistory addTime = new TimeHistory() 
            {
                //Schedule = timeHistoryVM.Schedule,
                //StartWork = timeHistoryVM.StartWork,
                //EndWork = timeHistoryVM.EndWork,
                //StartBreak = timeHistoryVM.StartBreak,
                //EndBreak = timeHistoryVM.EndBreak,
                //TotalWorkingHours = timeHistoryVM.TotalWorkingHours,
                //WorkingHours = timeHistoryVM.WorkingHours,
                //FlexiTime = timeHistoryVM.FlexiTime,
                //MoreTime = timeHistoryVM.MoreTime,
                //AttendanceTime = timeHistoryVM.AttendanceTime,
                //AnnualLeave = timeHistoryVM.AnnualLeave,
                //SickLeave = timeHistoryVM.SickLeave,
                //LeaveOfAbsence = timeHistoryVM.LeaveOfAbsence,
                //Childcare = timeHistoryVM.Childcare,
                //Overtime = timeHistoryVM.Overtime,
                //InconvenientHours = timeHistoryVM.InconvenientHours,
                //ProjectId = _db.Projects.FirstOrDefault(p => p.Id == timeHistoryVM.Users)!,



            };
        }

        public Task AddTimeHistoryAsync(TimeHistoryVM timeHistoryVM)
        {
            throw new NotImplementedException();
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

        public Task<Dictionary<string, double>> GenerateTimeReportByDaysAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<TimeHistory> GetTimeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        // Updating an existing time history 
        public async Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory)
        {
            _db.Update(updatedTimeHistory);
            await _db.SaveChangesAsync();
        }

        public Task UpdateTimeHistoryAsync(TimeHistoryVM timeHistoryVM)
        {
            throw new NotImplementedException();
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
