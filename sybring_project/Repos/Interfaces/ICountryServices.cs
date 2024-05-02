using sybring_project.Models;
using sybring_project.Models.Db;
using static sybring_project.Models.Db.CountriesSowAPI;

namespace sybring_project.Repos.Interfaces
{
    public interface ICountryServices
    {
       

        Task<Rootobject> GetAllCountriesAsync();

    }
}
