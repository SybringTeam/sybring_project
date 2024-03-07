using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class TimeService : ITimeService
    {
        private readonly ApplicationDbContext _context;

        public TimeService(ApplicationDbContext context)
        {
            _context = context;
        }



        // Adding a new time history 
        public async Task AddTimeHistoryAsync(TimeHistory timeHistory)
        {
            _context.TimeHistories.Add(timeHistory);
            await _context.SaveChangesAsync();
        }



        // Deleting a time history 
        public async Task DeleteTimeHistoryAsync(int id)
        {
            var timeHistoryToDelete = await _context.TimeHistories.FindAsync(id);
            if (timeHistoryToDelete != null)
            {
                _context.TimeHistories.Remove(timeHistoryToDelete);
                await _context.SaveChangesAsync();
            }


        }


        // Updating an existing time history 
        public async Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory)
        {
            _context.Update(updatedTimeHistory);
            await _context.SaveChangesAsync();
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
