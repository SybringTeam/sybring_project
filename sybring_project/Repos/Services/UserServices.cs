using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;
using System.Linq;

namespace sybring_project.Repos.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IProjectServices _projectServices;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public UserServices(ApplicationDbContext db,
            UserManager<User> userManager, IProjectServices projectServices, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _projectServices = projectServices;
            _configuration = configuration;
            _blobServiceClient = new BlobServiceClient(_configuration["AzureWebJobsStorage"]);
        }

        private BlobContainerClient InitBlobService(string blobContainer)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(blobContainer);
            return containerClient;
        }


        public async Task<string> UploadImage(IFormFile File)
        {
            BlobClient blobClient = InitBlobService("sybringsstorage").GetBlobClient(File.FileName);

            await using (var stream = File.OpenReadStream())
            {
                blobClient.Upload(stream);
            }

            return blobClient.Uri.AbsoluteUri;
        }
        public async Task<User> AddUsersAsync(User newUser, int projectId)
        {
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            if (projectId != 0)
            {
                var project = await _db.Projects.FindAsync(projectId);
                if (project != null)
                {
                    newUser.ProjectId = new List<Project> { project };
                    await _db.SaveChangesAsync();
                }
            }

            return newUser;
        }


        public async Task<User> DeleteUserAsync(string id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            var list = await _db.Users.Include(x => x.ProjectId).ToListAsync();
            return list;
        }


        public async Task<List<User>> GetAllUsersInRoleAsync(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.ToList();
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _db.Users
            .Include(u => u.ProjectId)
    .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            return user;

        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var userInDb = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                if (userInDb == null)
                {
                    // Handle case where the user doesn't exist
                    return false;
                }
                // Update properties
                userInDb.FirstName = user.FirstName;
                userInDb.LastName = user.LastName;
                userInDb.Age = user.Age;
                userInDb.Address = user.Address;
                userInDb.TaskDescription = user.TaskDescription;
                userInDb.UserIncome = user.UserIncome;
                userInDb.CitizenMembership = user.CitizenMembership;
                userInDb.DOB = user.DOB;
                userInDb.UserPersonalNumber = user.UserPersonalNumber;
                userInDb.ICEContactName = user.ICEContactName;
                userInDb.UserICE = user.UserICE;
                userInDb.Seller = user.Seller;
                userInDb.ImageLink = user.ImageLink;
                userInDb.Email = user.Email; // Update Email property

                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency exception
                return false;
            }
        }

                
                



        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _db.Projects.FindAsync(id);
        }

        public async Task<bool> RemoveUserFromProjectAsync(int projectId, string userId)
        {

            var existingProject = await _db.Projects
                 .Include(p => p.Users)
                 .FirstOrDefaultAsync(p => p.Id == projectId);

            if (existingProject != null)
            {

                var userToRemove = existingProject.Users
                    .FirstOrDefault(u => u.Id == userId);

                if (userToRemove != null)
                {

                    existingProject.Users.Remove(userToRemove);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }




        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }



        public async Task AssignProjectToUserAsync(string userId, int projectId)
        {
            var existingProject = await _db.Projects
               .Include(p => p.Users)
               .FirstOrDefaultAsync(p => p.Id == projectId);

            if (existingProject != null)
            {
                var userToAdd = _db.Users.FirstOrDefault(u => u.Id == userId);
                             existingProject.Users.Add(userToAdd);
                                existingProject.Users.Add(userToAdd);
                await _db.SaveChangesAsync();

            }
        }
        public async Task TaskManager(string userId, int projectId)
        {
            //var user = await GetUserByIdAsync(userId);
            //if (user == null)
            //{
            //    return;
            //}

            var existingUsers = await _db.Projects
              .Include(p => p.Users)
              .FirstOrDefaultAsync(p => p.Id == projectId);

            if (existingUsers != null)
            {
                var userToAdd = _db.Users.FirstOrDefault(u => u.Id == userId);
                existingUsers.Users.Add(userToAdd);
                existingUsers.Users.Add(userToAdd);
                await _db.SaveChangesAsync();

            }
            var existingProject = await _db.Users
                .Include(u => u.ProjectId)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var projectToAdd = _db.Projects.FirstOrDefault(p => p.Id == projectId);


            if (existingProject != null && projectToAdd != null)
            {
                existingProject.ProjectId.Add(projectToAdd);
                await _db.SaveChangesAsync();
            }



            //var project = await _projectServices.GetProjectByIdAsync(projectId);
            //if (project != null)
            //{
            //    // Assign project to user
            //    user.ProjectId.Add(project);
            //    await _db.SaveChangesAsync();
            //}
        }


        public User GetUserById(string userId)
        {
            return _db.Users.FirstOrDefault(u => u.Id == userId);
        }

    }
}

