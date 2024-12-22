using UserManagement.Models;

namespace UserManagement.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(Guid id);
        Task CreateUsuarioAsync(Usuario usuario);
        Task UpdateUsuarioAsync(Usuario usuario);
        Task DeleteUsuarioAsync(Guid id);
        Task<Usuario> GetUsuarioByUsernameAsync(string username);
    }
}