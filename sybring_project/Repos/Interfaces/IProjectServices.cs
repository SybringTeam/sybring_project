using sybring_project.Models.Db;

namespace sybring_project.Repos.Interfaces
{
    public interface IProjectServices
    {
        Task<List<Project>> GetProjectsAsync();

        Task AddProjectAsync(Project project);
        Task<Project> GetProjectByIdAsync(int id);

        Task<Project> DeleteProjectAsync(int id);

        Task<bool> UpdateProjectAsync(Project project);

        Task AssigUserToProjectAsync(string userId, int projectId);

        Task<List<string>> GetAllSupervisorsAsync(Company company);
        Task<Project> GetProjectByCompanyIdAsync(int companyId);


    }
}
