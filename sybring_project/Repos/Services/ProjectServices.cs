using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly ApplicationDbContext _db;

        public ProjectServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _db.Projects.FindAsync(id);
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.ToListAsync();
        }
    }
}
