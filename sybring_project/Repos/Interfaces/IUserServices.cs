using sybring_project.Models.Db;

namespace sybring_project.Repos.Interfaces
{
    public interface IUserServices
    {
        Task<List<User>> GetAllUserAsync();
        Task<User> AddUsersAsync(User newUser);

        Task<bool> UpdateUserAsync(User user);

        Task<User> GetUserByIdAsync(string id);

        Task<User> DeleteUserAsync(string id);

        Task<List<Project>> GetProjectsAsync();

        Task<User> RegisterUserAsync(User newUser, string password);
        Task<string> UploadImageFileAsync(User user);

        Task<Project> AssignTaskAsync(int projectId, string userId);
    }
}