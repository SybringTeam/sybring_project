using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;

namespace sybring_project.Repos.Interfaces
{
    public interface IBillingServices
    {
        Task<List<Billing>> GetBillingAsync(string userId);

        Task AddBillingAsync(BillingVM billingVM, string userId, int projectId);

        Task<Billing> GetBillingByIdAsync(int id);

        Task<Billing> DeleteBillingAsync(int id);

        Task<bool> UpdateCompanyAsync(Billing billing);

        Task BillingUserAsync(string userId, int billingId);

        Task<BillingVM> GetProjectsAndUsersAsync();



        Task<string> UploadImageFileAsync(BillingVM billingVM);

     


    }
}
