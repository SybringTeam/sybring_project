using sybring_project.Models;

namespace sybring_project.Repos.Interfaces
{
    public interface IHolidayService
    {
        Task<Holiday> GetHolidayReportAsync();

        Task<Holiday> GetHolidayDetails();

    }
}
