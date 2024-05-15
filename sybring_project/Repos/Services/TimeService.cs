using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using System.Threading.Tasks;
using System.Globalization;




namespace sybring_project.Repos.Services
{
    public class TimeService : ITimeService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<TimeService> _logger;
        private readonly IUserServices _userServices;




        public TimeService(ApplicationDbContext db, UserManager<User> userManager, ILogger<TimeService> logger, IUserServices userServices)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _userServices = userServices;
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



        //Retrieves a time history record by its ID from the database 
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

        public async Task<IEnumerable<TimeHistory>> GetHistoryByWeekNUser(string userId, string dateRange)
        {
            DateTime startDate;
            DateTime endDate = DateTime.Now;

            switch (dateRange.ToLower())
            {
                case "week":
                    startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
                    endDate = startDate.AddDays(6); // Last day of the week
                    break;
                default:
                    throw new ArgumentException("Invalid date range for weekly history retrieval");
            }

            var currentUser = await _userManager.FindByIdAsync(userId);
            IQueryable<TimeHistory> query;

            if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                // If the user is an admin, retrieve time histories for any user within the specified week
                query = _db.TimeHistories
                    .Include(t => t.Users)
                    .Where(t => t.Date >= startDate && t.Date <= endDate);
            }
            else
            {
                // If the user is not an admin, retrieve time histories only for the current user within the specified week
                query = _db.TimeHistories
                    .Include(t => t.Users)
                    .Where(t => t.Users.Any(u => u.Id == userId) && t.Date >= startDate && t.Date <= endDate);
            }

            var timeHistories = await query.ToListAsync();

            return timeHistories;
        }

        //Edan Details by month
        public async Task<IEnumerable<TimeHistory>> GetTimeHistoriesForMonthAsync(string month)
        {
            // Parse the month string to get the year and month components
            if (!DateTime.TryParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedMonth))
            {
                throw new ArgumentException("Invalid month format.");
            }

            var startDate = new DateTime(parsedMonth.Year, parsedMonth.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the month

            // Retrieve time histories for the specified month from the database
            var timeHistories = await _db.TimeHistories
                .Include(t => t.Users)
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToListAsync();

            return timeHistories;
        }

        //Spurti
        //TimeTrigger method for Delete five years data

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






        //spurti


        public async Task<IEnumerable<User>> GetUsersWithoutTimeReportForPreviousWeek()
        {
            // Calculate the date range for the previous week
            DateTime startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(-7);
            DateTime endDate = startDate.AddDays(6);

            // Query for users who don't have time history records for the previous week
            var usersWithoutTimeReport = await _userManager.Users
                .Where(user => !_db.TimeHistories.Any(time => time.Users.Any(u => u.Id == user.Id) && time.Date >= startDate && time.Date <= endDate))
                .ToListAsync();

            return usersWithoutTimeReport;
        }









        // Method to check if time reports exist for the previous week
        private async Task<bool> HasFilledTimeReportForPreviousWeek(string userId)
        {
            DateTime startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(-7);
            DateTime endDate = startDate.AddDays(6);

            return await _db.TimeHistories
                .AnyAsync(t => t.Users.Any(u => u.Id == userId) && t.Date >= startDate && t.Date <= endDate);
        }

        // TimeTrigger method for missing time report
        public async Task ForgetTimeReport(Func<string, string, string, Task> sendEmailAsync)
        {
            // Get all users
            var users = await _userServices.GetAllUserAsync();

            foreach (var user in users)
            {
                // Check if the user has filled their time report for the previous week
                if (!await HasFilledTimeReportForPreviousWeek(user.Id))
                {
                    // Send email reminders to users who haven't filled their time reports
                    string userEmail = user.Email;
                    string subject = "Reminder: Fill out your time report";
                    string body = $"Dear {user.FirstName},\n\nThis is a friendly reminder to fill out your time report for the previous week. Please make sure to complete it at your earliest convenience.\n\nBest regards,\nThe Sybring Team";

                    // Call method to send the email
                    await sendEmailAsync(userEmail, subject, body);

                    Console.WriteLine($"Email reminder sent to {userEmail}");
                }
            }
        }


    }
}