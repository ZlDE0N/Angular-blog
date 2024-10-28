using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace BackendBlogServicesApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null);
        Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
        Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    }
}
