using sybring_project.Models.Db;

namespace sybring_project.Repos.Interfaces
{
    public interface IUserServices
    {
        Task<List<User>> GetAllUserAsync();
        Task<User> AddUsersAsync(User newUser);

        Task<bool> UpdateUserAsync(User user);

        Task<User> GetUserByIdAsync(int id);

        Task<User> DeleteUserAsync(int id);

        Task<List<Project>> GetProjectsAsync();

        Task<string> UploadImageFileAsync(User user);
    }
}
