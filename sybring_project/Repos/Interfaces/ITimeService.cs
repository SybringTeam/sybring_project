
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;




namespace sybring_project.Repos.Interfaces
{
    public interface ITimeService
    {
        // Methods to add , delete and update a new time history record

        Task<List<TimeHistory>> GetTimeListAsync();
        Task AddTimeHistoryAsync(TimeHistory timeHistory);

        Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory);
        Task DeleteTimeHistoryAsync(int id);

        Task<TimeHistory> GetTimeHistoryByIdAsync(int id);

        Task AddReportAsync(DayDataVM dayDataVM);


        Task<List<decimal>> CalculateWeekDataAsync(TimeReportViewModel timeReportViewModel);

        Task AssigUserToTimeAsync(string userId, int timeID);
        decimal CalculateOvertime(decimal workingHours, decimal maxRegularHoursPerDay);
        decimal CalculateWorkingHoursAsync(TimeSpan startTime, TimeSpan endTime);


        //Task<ProjectTimeReport> ProjectWorkingHoursAsync();

        //Task<Dictionary<string, double>> GenerateTimeReportByDaysAsync(DateTime startDate, DateTime endDate);


    }
}