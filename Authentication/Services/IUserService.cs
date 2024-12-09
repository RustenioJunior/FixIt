using System.Collections.Generic;
using System.Threading.Tasks;
using Authentication.Models;

namespace Authentication.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetUsers();
        Task<User> Create(User user, string password);
    }
}