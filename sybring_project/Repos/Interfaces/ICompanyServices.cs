using sybring_project.Models.Db;
using System.Runtime.InteropServices;

namespace sybring_project.Repos.Interfaces
{
    public interface ICompanyServices
    {
        Task<List<Company>> GetCompanyAsync();

        Task AddCompanyAsync(Company company);

        Task<Company> GetCompanyByIdAsync(int id);

        Task<Company> DeleteCompanyAsync(int id);

        Task<bool> UpdateCompanyAsync(Company company);

       
    }
}
