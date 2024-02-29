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
            var list = await _db.Users.Include(x => x.ProjectId).ToListAsync();
            return list;
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


        public Task<string> UploadImageFileAsync(User user)
        {
            throw new NotImplementedException();
        }

        //public async Task<Project> AssignTaskAsync(int projectId, string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    var project = await _db.Projects.FindAsync(projectId);

        //    if (user != null && project != null)
        //    {
        //        var projectUserVM = new ProjectUserVM
        //        {
        //            ProjectId = projectId,
        //            UserId = userId
        //        };

        //        // Convert the view model to the entity
        //        var projectUser = new ProjectUserVM
        //        {
        //            ProjectId = projectUserVM.ProjectId,
        //            UserId = projectUserVM.UserId
        //        };

        //        user.ProjectUsers.Add(projectUser);
        //        project.UserProjects.Add(projectUser);

        //        await _db.SaveChangesAsync();
        //        return project;
        //    }



        }
    }

