using sybring_project.Models.Db;

namespace sybring_project.Repos.Interfaces
{
    public interface ITimeService
    {
        // Methods to add , delete and update a new time history record
        Task AddTimeHistoryAsync(TimeHistory timeHistory);

        Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory);
        Task DeleteTimeHistoryAsync(int id);


       

    }
}
