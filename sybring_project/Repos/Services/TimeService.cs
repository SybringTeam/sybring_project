using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class TimeService : ITimeService
    {
        private readonly ApplicationDbContext _context;

        public TimeService(ApplicationDbContext context)
        {
            _context = context;
        }



        // Adding a new time history 
        public async Task AddTimeHistoryAsync(TimeHistory timeHistory)
        {
            _context.TimeHistories.Add(timeHistory);
            await _context.SaveChangesAsync();
        }



        // Deleting a time history 
        public async Task DeleteTimeHistoryAsync(int id)
        {
            var timeHistoryToDelete = await _context.TimeHistories.FindAsync(id);
            if (timeHistoryToDelete != null)
            {
                _context.TimeHistories.Remove(timeHistoryToDelete);
                await _context.SaveChangesAsync();
            }


        }


        // Updating an existing time history 
        public async Task UpdateTimeHistoryAsync(TimeHistory updatedTimeHistory)
        {
            _context.Update(updatedTimeHistory);
            await _context.SaveChangesAsync();
        }


      


}
}
