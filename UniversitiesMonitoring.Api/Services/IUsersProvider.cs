

using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

internal interface IUsersProvider
{
    Task<User?> GetUserAsync(ulong userId);
    Task<bool> ModifyUserAsync(ulong userId, Action<User> modifyAction);
    Task<User> CreateUserAsync(string username, string password);
}


