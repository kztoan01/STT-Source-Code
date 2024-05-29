using Microsoft.AspNetCore.Mvc;
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
            var user = await _loginService.LoginAsync(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // Tạo token hoặc session cho người dùng ở đây nếu cần thiết
            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
