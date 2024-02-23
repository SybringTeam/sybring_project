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

        Task<string> UploadImageFileAsync(User user);
    }
}