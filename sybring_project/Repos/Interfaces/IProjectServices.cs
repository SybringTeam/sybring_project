using sybring_project.Models.Db;

namespace sybring_project.Repos.Interfaces
{
    public interface IProjectServices
    {
        Task<List<Project>> GetAllProjectAsync();

        Task<Project> AddProjectsAsync(Project newProject);

    }
}
