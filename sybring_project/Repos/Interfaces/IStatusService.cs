﻿using sybring_project.Models.Db;

namespace sybring_project.Repos.Interfaces
{
    public interface IStatusService
    {
        Task UpdateUserAsync(User user);

        Task UpdateUserStatusAsync(string userId, string statusName);

        Task<List<Status>> GetStatusListAsync();

        Task<Status> GetStatusByIdAsync(int id);

        Task<Status> DeleteStatusAsync(int id);
    }
}
