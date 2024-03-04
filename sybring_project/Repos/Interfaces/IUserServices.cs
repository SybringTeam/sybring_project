using Microsoft.AspNetCore.Identity;
using sybring_project.Models.Db;
using sybring_project.Models.ViewModels;

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

        //Task<Project> AssignProjectToUserAsync(string userId, int projectId);

        Task<Project> GetProjectByIdAsync(int id);

        //Task<User> GetUserWithProjectsAsync(string id);


    }
}