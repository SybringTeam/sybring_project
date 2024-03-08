using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;

namespace sybring_project.Repos.Interfaces
{
    public interface ITimeService
    {
        // Methods to add , delete and update a new time history record
        Task AddTimeHistoryAsync(TimeHistoryVM timeHistoryVM);

        Task UpdateTimeHistoryAsync(TimeHistoryVM timeHistoryVM);

        Task<TimeHistory> GetTimeByIdAsync(int id);
        Task DeleteTimeHistoryAsync(int id);



        //Task<Dictionary<string, double>> GenerateTimeReportByDaysAsync(DateTime startDate, DateTime endDate);


    }
}
