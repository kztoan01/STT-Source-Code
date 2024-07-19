using core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using service.Service;

namespace controller.Controllers;

[Route("music-service/api/[controller]")]
[ApiController]
public class ListennerController : ControllerBase
{
    private readonly UserService _userService;
    private readonly UserManager<User> _userManager;

    public ListennerController(UserService userService,UserManager<User> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }
    [HttpPost("follow/{artistId}")]
    public async Task<IActionResult> Follow([FromRoute]Guid artistId)
    {
        var user = await _userManager.GetUserAsync(User);
        return Ok(await _userService.FollowArtist(new Guid(user.Id), artistId));
    }
    
    [HttpPost("unfollow/{artistId}")]
    public async Task<IActionResult> UnFollow([FromRoute]Guid artistId)
    {
        var user = await _userManager.GetUserAsync(User);
        return Ok(await _userService.UnFollowArtist(new Guid(user.Id), artistId));
    }
}