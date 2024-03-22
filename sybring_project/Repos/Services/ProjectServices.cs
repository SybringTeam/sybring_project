using Microsoft.Build.ObjectModelRemoting;
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

        public async Task AssigUserToProjectAsync(string userId, int projectId)
        {
            var existingUser = await _db.Users
                .Include(u => u.ProjectId)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var projectToAdd = _db.Projects.FirstOrDefault(p => p.Id == projectId);


            if (existingUser != null && projectToAdd != null)
            {
                existingUser.ProjectId.Add(projectToAdd);
                await _db.SaveChangesAsync();
            }

        }

        public async Task<Project> DeleteProjectAsync(int id)
        {
            var del = await _db.Projects.FindAsync(id);

            _db.Projects.Remove(del);
             await _db.SaveChangesAsync();

            return del;
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var project = await _db.Projects.Include(p => p.Users)
                                             .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                throw new InvalidOperationException($"Project with ID {id} not found.");
            }

            return project;
        }


        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.Include(p => p.Users).ToListAsync();
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            try
            {
                _db.Entry(project).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;
            }
        }
        
    }
}
