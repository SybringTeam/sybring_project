using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;

namespace sybring_project.Repos.Interfaces
{
    public interface IBillingServices
    {
        Task<List<Billing>> GetBillingAsync();

        Task AddBillingAsync(BillingVM billingVM, string userId);

        Task<Billing> GetBillingByIdAsync(int id);

        Task<Billing> DeleteCompanyAsync(int id);

        Task<bool> UpdateCompanyAsync(Billing billing);

        Task BillingUserAsync(string userId, int billingId);

        Task<ProjectBillingCompanyVM> GetProjectsAndUsersAsync();



        Task<string> UploadImageFileAsync(BillingVM billingVM);

    }
}
