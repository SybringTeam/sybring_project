using sybring_project.Models;

namespace sybring_project.Repos.Interfaces
{
    public interface IHoildayService
    {
        Task<Hoilday> GetHoildayReport();

    }
}
