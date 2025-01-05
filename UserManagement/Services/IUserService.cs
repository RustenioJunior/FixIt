using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserById(string id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> UpdateUser(string id, User updatedUser);
        Task<bool> DeleteUser(string id);
         Task<bool> ValidateToken(string token);
    }
}