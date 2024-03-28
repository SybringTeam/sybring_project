using sybring_project.Models;
using sybring_project.Models.Db;
namespace sybring_project.Repos.Interfaces
{
    public interface IHolidayService
    {
        Task<Holiday> GetHolidayReportAsync();

        Task<Holiday> GetHolidayDetails();


        Task<IEnumerable<Holiday>> GetRedDaysAsync();

      

        
    }


}

