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
           const decimal standardWorkingHoursPerDay = 8;

            var timeReport = new TimeHistory
            {
                Date = dayDataVM.Date,
                StartWork = dayDataVM.StartWork,
                EndWork = dayDataVM.EndWork,
                StartBreak = dayDataVM.StartBreak,
                EndBreak = dayDataVM.EndBreak,
                //TotalWorkingHours = totalWorkingHours,
                TotalWorkingHours = dayDataVM.TotalWorkingHours,
                //WorkingHours = dayDataVM.WorkingHours,
                //WorkingHours = standardWorkingHoursPerDay,
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
            //Checking if StartWork, EndWork, StartBreak, and EndBreak are null or have default values
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

                // Calculate Total Working Hours (TWH)
                decimal totalWorkingHours = workingHours + overtime;

                // Calculate Inconvenient Time (Summary)
                decimal inconvenientTime = workingHours + overtime;

                dayDataVM.Overtime = overtime;
                dayDataVM.TotalWorkingHours = totalWorkingHours;
                dayDataVM.InconvenientHours = inconvenientTime;

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





        //Spurti
        //TimeTrigger method

        public void DeleteOldData()
        {
            try
            {
                // current date
                DateTime fiveYearsAgo = DateTime.UtcNow.AddYears(-5);

                // Query 
                var oldData = _db.TimeHistories.Where(item => item.Date <= fiveYearsAgo);


                // Remove
                _db.TimeHistories.RemoveRange(oldData);

                // Save changes 
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                //  exception
                Console.WriteLine($"An error occurred while deleting old data: {ex.Message}");
            }
        }







    }
}