using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}")]
         [Authorize]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

             if(string.IsNullOrEmpty(token)) return Unauthorized();

           var isTokenValid =  await _userService.ValidateToken(token);

           if(!isTokenValid) return Unauthorized();

             var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(string id, User updatedUser)
        {
            var user = await _userService.UpdateUser(id, updatedUser);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var deleted = await _userService.DeleteUser(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}