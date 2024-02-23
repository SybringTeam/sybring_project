using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _db;

        public UserServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User> AddUsersAsync(User newUser)
        {
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
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
            return await _db.Users.Include(x => x.ProjectId).ToListAsync();
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {

            return await _db.Users.FindAsync(id);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues if necessary
                return false;
            }
        }

        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}