using Azure.Storage.Blobs;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Project = sybring_project.Models.Db.Project;

namespace sybring_project.Repos.Services
{
    public class BillingServices : IBillingServices
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IProjectServices _projectServices;


        public BillingServices(ApplicationDbContext db, IConfiguration configuration,
            IProjectServices projectServices)
        {
            _db = db;
            _configuration = configuration;
            _projectServices = projectServices;
        }

        public async Task AddBillingAsync(BillingVM billingVM, string userId, int projectId)
        {
            Billing billing = new Billing()
            {
                Id = billingVM.Id,
                Description = billingVM.Description,
                ImageLink = billingVM.ImageLink,
                Cost = billingVM.Cost,
                DateStamp = billingVM.DateStamp,
              
                ProjectId = new List<Project>(),
                Users = new List<User>()


            };
           
            var project = await _db.Projects.FindAsync(projectId);
            billing.ProjectId = new List<Project> { project };

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

        public async Task<Billing> DeleteBillingAsync(int id)
        {
            var delBilling = await _db.Billings.FindAsync(id);

            var associatedProject = _db.Projects.Where(p => p.BillingId == id);
            foreach (var item in associatedProject)
            {
                item.BillingId = null;
            }
            _db.Billings.Remove(delBilling);
            await _db.SaveChangesAsync();
            return delBilling;
        }

        public async Task<List<Billing>> GetBillingAsync(string userId)
        {
            var billingList = await _db.Billings
                               .Include(b => b.Users)
                               .Where(b => b.Users.Any(u => u.Id == userId))
                               .ToListAsync();
            return billingList;
        }

        public async Task<Billing> GetBillingByIdAsync(int id)
        {
            var billing = await _db.Billings.Include(b => b.ProjectId)
                .Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.Id == id);
            return billing;
        }

        public async Task<bool> UpdateCompanyAsync(Billing billing)
        {
            _db.Update(billing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<BillingVM> GetProjectsAndUsersAsync()
        {
            var projects = await _db.Projects.ToListAsync();
            //var companies = await _db.Companies.ToListAsync();
            var users = await _db.Users.ToListAsync();

            return new BillingVM
            {
                ProjectId = projects,
                Users = users,

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
