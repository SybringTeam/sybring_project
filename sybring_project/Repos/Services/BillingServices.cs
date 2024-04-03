using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace sybring_project.Repos.Services
{
    public class BillingServices : IBillingServices
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;


        public BillingServices(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public async Task AddBillingAsync(BillingVM billingVM, string userId)
        {
            Billing billing = new Billing() 
            {
                Id = billingVM.Id,
                Description = billingVM.Description,
                ImageLink = billingVM.ImageLink,
                Cost = billingVM.Cost,
                DataStamp = billingVM.DateStamp

            };
            var user = await _db.Users.FindAsync(userId);

            billing.Users = new List<User> { user };

            await _db.AddAsync(billing);
            await _db.SaveChangesAsync();
        }

        public async Task BillingUserAsync(string userId, int billingId)
        {
            var user = await _db.Users.FindAsync(userId);
            var billing = await _db.Billings.FindAsync(billingId);

            if (user != null && billing != null)
            {
                user.ReceiptId.Add(billing);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Billing> DeleteCompanyAsync(int id)
        {
            var delBilling = await _db.Billings.FindAsync(id);
            _db.Billings.Remove(delBilling);
            await _db.SaveChangesAsync();
            return delBilling;
        }

        public async Task<List<Billing>> GetBillingAsync()
        {
            return _db.Billings.ToList();
        }

        public async Task<Billing> GetBillingByIdAsync(int id)
        {
            var billing = await _db.Billings.FindAsync(new { id });
            return billing;
        }

        public async Task<bool> UpdateCompanyAsync(Billing billing)
        {
            _db.Update(billing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectBillingCompanyVM> GetProjectsAndUsersAsync() 
        {
            var projects = await _db.Projects.ToListAsync();
            var companies = await _db.Companies.ToListAsync();
            var users = await _db.Users.ToListAsync();

            return new ProjectBillingCompanyVM
            {
                Projects = projects,
                Users = users,
                Companies = companies
            };
        }

        public async Task<string> UploadImageFileAsync(BillingVM billingVM)
        {
            IFormFile file = billingVM.File;
            string uniqueFileName = billingVM.ImageLink;
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                _configuration["AzureWebJobsStorage"]);
            BlobContainerClient blobContainerClient = blobServiceClient
                .GetBlobContainerClient("sybringsstorage");
            BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueFileName);

            using (var stream = file.OpenReadStream()) 
            {
                blobClient.Upload(stream);
            }
            return blobClient.Uri.AbsoluteUri;
        }


        private Uri GetBlobImage(string imgLink)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                _configuration["AzureWebJobsStorage"]);
            var blobClient = blobServiceClient.GetBlobContainerClient("sybringsstorage");
            var address = blobClient.GetBlobClient(imgLink).Uri;
            return address;
        }
    }
}
