using UserManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace UserManagement.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _usuarios = mongoDatabase.GetCollection<Usuario>("Usuarios");
        }
 public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _usuarios.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(Guid id)
        {
            return await _usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateUsuarioAsync(Usuario usuario)
        {
            await _usuarios.InsertOneAsync(usuario);
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            await _usuarios.ReplaceOneAsync(u => u.Id == usuario.Id, usuario);
        }

        public async Task DeleteUsuarioAsync(Guid id)
        {
            await _usuarios.DeleteOneAsync(u => u.Id == id);
        }

        public async Task<Usuario> GetUsuarioByUsernameAsync(string username)
        {
            return await _usuarios.Find(u => u.Username == username).FirstOrDefaultAsync();
        }
    }
}