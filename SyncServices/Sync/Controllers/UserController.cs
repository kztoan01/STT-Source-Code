using Microsoft.AspNetCore.Mvc;
using Sync.DTOs;
using Sync.Services;

namespace Sync.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> getUsers()
        {
            var user = _UserService.getAllUsers();

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }


        [HttpGet("GetUserByID")]
        public async Task<IActionResult> GetUserByID(Guid id)
        {
            var user = _UserService.getById(id);

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }

        [HttpGet("GetUserByName")]
        public async Task<IActionResult> GetUserByName(String name)
        {
            var user = _UserService.getByName(name);

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }



        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
        {
            var user = _UserService.Update(userDTO);

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] UserDTO userDTO)
        {
            var user = _UserService.Delete(userDTO);

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }




    }
}

