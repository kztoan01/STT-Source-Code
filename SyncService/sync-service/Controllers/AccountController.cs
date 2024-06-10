using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sync_service.Dtos.Account;
using sync_service.Models;
using sync_service.Service.Interfaces;

namespace sync_service.Controllers
{
    [Route("music-service/api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO) 
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email
                };
                
                var getUserEmail = await _userManager.FindByEmailAsync(registerDTO.Email);

                if(getUserEmail != null)
                {
                    return StatusCode(500, "Email exist");
                }

                var createdUser = await _userManager.CreateAsync(user, registerDTO.Password);

                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDTO
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
                    } 
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else 
                {
                    return StatusCode(500, createdUser.Errors);
                }
            } 
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var getUser = await _userManager.FindByEmailAsync(loginDto.Email);

            if(getUser == null)
            {
                return Unauthorized("User not found");
            }

            bool checkUserPassword = await _userManager.CheckPasswordAsync(getUser, loginDto.Password);

            if(!checkUserPassword)
            {
                return Unauthorized("Wrong Password");
            }
            else 
            {
                var getUserRole = await _userManager.GetRolesAsync(getUser);
                var userSession = new UserSession(getUser.Id,getUser.UserName,getUser.Email,getUserRole.First());
                return Ok(
                    new LoginSuccessDTO
                    {
                        UserName = getUser.UserName,
                        Email = getUser.Email,
                        //Token = _tokenService.CreateToken(getUser)
                        Token = _tokenService.GenerateToken(userSession)
                    }
                );
            }

            //var getUserRole = await _userManager.GetRolesAsync(getUser);
            /*
            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "Login completed");
            */

        }
    }
}