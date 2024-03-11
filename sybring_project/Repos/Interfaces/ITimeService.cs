using Microsoft.AspNetCore.Mvc;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;

namespace sybring_project.Repos.Interfaces
{
    public interface ITimeService
    {
        // Methods to add , delete and update a new time history record
        Task AddTimeHistoryAsync(TimeHistory timeHistory);

        Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory);
        Task DeleteTimeHistoryAsync(int id);

        Task AddReportAsync(TimeReportViewModel model);

        decimal CalculateWorkingHoursAsync(TimeSpan startTime, TimeSpan endTime);


        //Task<Dictionary<string, double>> GenerateTimeReportByDaysAsync(DateTime startDate, DateTime endDate);


    }
}