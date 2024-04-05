using Microsoft.AspNetCore.Http.HttpResults;
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
                                               .Include(p => p.Companies)

                                             .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                throw new InvalidOperationException($"Project with ID {id} not found.");
            }

            return project;
        }



        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.Include(p => p.Users)
                .ToListAsync();
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



        public async Task<List<string>> GetAllSupervisorsAsync(Company company)
        {
            var companyWithSupervisorNames = await _db.Companies
                .Where(c => c.Id == company.Id)
                .Select(c => c.SupervisorName)
                .FirstOrDefaultAsync();

            var supervisors = companyWithSupervisorNames.Split(',').ToList();
            return supervisors;
        }

        public async Task<List<Project>> GetProjectsByCompanyIdAsync(int companyId)
        {
            var project = await _db.Projects
                    .Where(p => p.CompanyId == companyId)
                    .ToListAsync();

            return project;
        }


        public async Task<List<User>> GetAssignedUserForProjectAsync(int projectId)
        {
            var user = await _db.Users
           .Include(u => u.ProjectId)
           .Where(u => u.ProjectId.Any(p => p.Id == projectId))
           .ToListAsync();

            return user;
        }
    }



}
