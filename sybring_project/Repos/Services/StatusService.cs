
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserServices _userServices;

        public StatusService(ApplicationDbContext db, IUserServices userServices)
        {
            _db = db;
            _userServices = userServices;
        }

        public async Task AddStatusAsync(Status status)
        {
            _db.Status.Add(status);
            await _db.SaveChangesAsync();


        }

        public async Task<Status> DeleteStatusAsync(int id)
        {
            var statusToDelete = await _db.Status.FindAsync(id);

            if (statusToDelete != null)
            {
                // Remove references from users
                var usersWithStatus = await _db.Users
                    .Where(u => u.Status.Any(s => s.Id == id)).ToListAsync();

                _db.Users.RemoveRange(usersWithStatus);
                _db.Status.Remove(statusToDelete);
                await _db.SaveChangesAsync();
            }

            return statusToDelete;
        }

        public async Task<Status> GetStatusByIdAsync(int id)
        {
            return await _db.Status.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Status>> GetStatusListAsync()
        {

            return await _db.Status.ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateUserStatusAsync(string userId, string statusName)
        {
            // Find the user by userId
            var user = await _db.Users.FindAsync(userId);

            // Find the status by name
            var status = await _db.Status.FirstOrDefaultAsync(s => s.Name == statusName);

            // Update the user's status
            //user.Status = status;

            // Save changes to the database
            await _db.SaveChangesAsync();
        }



    }
}
