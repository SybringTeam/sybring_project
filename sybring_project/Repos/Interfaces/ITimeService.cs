﻿
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;




namespace sybring_project.Repos.Interfaces
{
    public interface ITimeService
    {
        // Methods to add , delete and update a new time history record

        Task<List<TimeHistory>> GetTimeListAsync(string userId);
        Task AddTimeHistoryAsync(TimeHistory timeHistory);

        Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory);
        Task DeleteTimeHistoryAsync(int id);

        Task<TimeHistory> GetTimeHistoryByIdAsync(int id);

        Task AddReportAsync(DayDataVM dayDataVM, string userId, decimal scheduledHoursPerWeek);


        //Task<List<decimal>> CalculateWeekDataAsync(TimeReportViewModel timeReportViewModel);

       
        //decimal CalculateOvertime(decimal workingHours, decimal maxRegularHoursPerDay);
        decimal CalculateWorkingHoursAsync(DayDataVM dayDataVM, decimal scheduledHoursPerWeek);


        //Task<ProjectTimeReport> ProjectWorkingHoursAsync();

        //Task<Dictionary<string, double>> GenerateTimeReportByDaysAsync(DateTime startDate, DateTime endDate);


        Task<IEnumerable<TimeHistory>> GetTimeHistoriesAsync(string userId, string dateRange);
        Task<IEnumerable<TimeHistory>> GetHistoryByWeekNUser(string userId, string dateRange);
        Task<IEnumerable<TimeHistory>> GetTimeHistoriesForMonthAsync(string month);

        void DeleteOldData();  //TimeTrigger method




       


        Task ForgetTimeReport(Func<string, string, string, Task> sendEmailAsync); //TimeTrigger method


        Task<IEnumerable<User>> GetUsersWithoutTimeReportForPreviousWeek();



    }
}