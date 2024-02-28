using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IProjectServices _projectServices;
        public UserServices(ApplicationDbContext db, 
            UserManager<User> userManager, IProjectServices projectServices)
        {
            _db = db;
            _userManager = userManager;
            _projectServices = projectServices;
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
                
                return false;
            }
        }


        public async Task<User> RegisterUserAsync(User newUser, string password)
        {
            var regUser = await _userManager.CreateAsync(newUser, password);

            if (regUser.Succeeded)
            {
                return newUser;
            }
            else
            {
                throw new ApplicationException("User registration failed. Check the provided information.");
            }
        }

        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> AssignTaskAsync(int projectId, string userId)
        {
            var project = await _db.Projects.FindAsync(projectId);

            if (project == null)
            {
                return null;

            }
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
               
                return null;
            }

            project.Users.Add(user);

            await _db.SaveChangesAsync();

            return project;
        }
    }
}