using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var usuarios = await _userRepository.GetUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            // Validações dos dados do usuário
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Hash da senha
            usuario.Password = _authService.HashSenha(usuario.Password);

            await _userRepository.CreateUsuarioAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(Guid id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            // Validações dos dados do usuário
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userRepository.UpdateUsuarioAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            var usuario = await _userRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUsuarioAsync(id);
            return NoContent();
        }

    }
}