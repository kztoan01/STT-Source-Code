using Microsoft.AspNetCore.Mvc;
using Sync.DTOs;
using Sync.Services;

namespace Sync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = _loginService.Login(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var user = _loginService.Register(userDTO);

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}



