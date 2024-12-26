using Authentication.Models;

namespace Authentication.Repositories
{
    public interface IUserRepository
    {
        Task<List<Usuario>> GetUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(Guid id);
        Task CreateUsuarioAsync(Usuario usuario);
        Task UpdateUsuarioAsync(Usuario usuario);
        Task DeleteUsuarioAsync(Guid id);
        Task<Usuario> GetUserByUsernameAsync(string username);
    }
}