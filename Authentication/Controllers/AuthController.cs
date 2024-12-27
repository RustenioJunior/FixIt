using Authentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Authentication.Repositories;


namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserRepository _userRepository; 


        public AuthController(IAuthenticationService authService, IUserRepository userRepository) => (_authService, _userRepository) = (authService, userRepository);

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AuthenticatedUserDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username); //Simplified user model
            if (user == null || !_authService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var token = _authService.GenerateJwtToken(user);
            return Ok(token);
        }

        //Optional: Endpoint for retrieving user details *after* successful login
        [HttpGet("user")]
        [Authorize] //Requires authentication
        public async Task<ActionResult<AuthenticatedUserDto>> GetAuthenticatedUser()
        {
            var username = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(); //Should not happen due to [Authorize] but good practice
            }
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return Ok(new AuthenticatedUserDto { Username = user.Username }); //Return only safe data
        }
    }
}

