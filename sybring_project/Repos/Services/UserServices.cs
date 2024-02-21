using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserServices> _logger;

        public UserServices(ApplicationDbContext db, ILogger<UserServices> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> AddUsersAsync(User newUser)
        {
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError($"Concurrency exception while updating user with ID {user.Id}");
                return false;
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _db.Users.FindAsync(id);
            
        }


        public async Task<User> DeleteUserAsync(int id)
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

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.ToListAsync();
        }


        public async Task<string> UploadImageFileAsync(User user)
        {
            //if (user.File == null)
            //{
            //    return null;
            //}

            //// Generate a unique filename for the image
            //string fileName = $"{user.Id}_{user.File.FileName}";

            //// Save the image to the blob storage
            //await user.File.CopyToAsync(new FileStream($"{_environment.WebRootPath}/images/{fileName}", FileMode.Create));

            //// Set the ImageLink property of the user to the new filename
            //user.ImageLink = $"/images/{fileName}";

            //// Save the changes to the database
            //await UpdateUserAsync(user);

            return user.ImageLink;
        }
    }
}