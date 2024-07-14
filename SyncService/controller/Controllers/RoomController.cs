using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using service.Hub;
using service.Hub.iml;

namespace controller.Controllers;
[Route("music-service/api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IHubContext<RoomHub, IRoomHub> _hubContext;

    public RoomController(IHubContext<RoomHub, IRoomHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("joinroom")]
    public async Task<IActionResult> JoinRoom([FromBody]string groupName,string userName)
    {
        await _hubContext.Clients.Group(groupName).AlertToRoom(userName);
        return Ok();
    }
}