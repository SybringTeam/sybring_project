using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class UserServices : IUserServices
    {
        private ApplicationDbContext _db;
        private IConfiguration _configuration;
        public UserServices(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<User> AddUsersAsync(User newUser)
        {
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return newUser;
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

        public async Task<List<User>> GetAllUserAsync()
        {
            var allUser = await _db.Users.Include(x =>x.ProjectId )             
                .ToListAsync();
            return allUser;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            var projects = await _db.Projects.ToListAsync();
            return projects;
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
