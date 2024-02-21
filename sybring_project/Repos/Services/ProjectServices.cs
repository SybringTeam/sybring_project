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

      
        public async Task<List<Project>> GetAllProjectAsync()
        {
            return await _db.Projects.ToListAsync();
        }


        public async Task<Project> AddProjectsAsync(Project newProject) 
        {
            _db.Projects.Add(newProject);
            await _db.SaveChangesAsync();
            return newProject;
        }

    }
}
