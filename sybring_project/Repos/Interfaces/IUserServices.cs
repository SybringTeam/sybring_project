using sybring_project.Models.Db;


namespace sybring_project.Repos.Interfaces
{
    public interface IUserServices
    {
        Task<List<User>> GetAllUserAsync();
        Task<User> AddUsersAsync(User newUser, int projectId);

        Task<bool> UpdateUserAsync(User user);

        Task<User> GetUserByIdAsync(string id);

        Task<User> DeleteUserAsync(string id);

        Task<List<Project>> GetProjectsAsync();


        Task<string> UploadImageFileAsync(User user);

       
        Task<Project> GetProjectByIdAsync(int id);

        Task<bool> RemoveUserFromProjectAsync(int projectId, string userId);

       
        Task AssignProjectToUserAsync(string userId, int projectId);

        Task TaskManager(string userId, int projectId);

        //Task<User> GetUserWithProjectsAsync(string id);
        User GetUserById(string userId);

    }
}