using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly AuthService _authService;

        public UsuarioController(IUsuarioRepository usuarioRepository, AuthService authService)
        {
            _usuarioRepository = usuarioRepository;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.GetUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
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

            await _usuarioRepository.CreateUsuarioAsync(usuario);
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

            await _usuarioRepository.UpdateUsuarioAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            await _usuarioRepository.DeleteUsuarioAsync(id);
            return NoContent();
        }

        [HttpPost("auth")]
        public async Task<ActionResult<string>> Autenticar(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetUsuarioByUsernameAsync(loginDto.Username);
            if (usuario == null || !_authService.VerificarSenha(loginDto.Password, usuario.Password))
            {
                return Unauthorized();
            }

            var token = _authService.GerarTokenJWT(usuario);
            return Ok(token);
        }
    }
}