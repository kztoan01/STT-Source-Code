using controller.Hubs;
using core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using service.Service.Interfaces;

namespace controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IHubContext<SignalRServer> _hubContext;

        public RoomController(IRoomService roomService, IHubContext<SignalRServer> hubContext)
        {
            _roomService = roomService;
            _hubContext = hubContext;
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomById(Guid roomId)
        {
            var room = await _roomService.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {
            await _roomService.AddRoomAsync(room);
            return CreatedAtAction(nameof(GetRoomById), new { roomId = room.Id }, room);
        }

        [HttpPut("{roomId}")]
        public async Task<IActionResult> UpdateRoom(Guid roomId, [FromBody] Room room)
        {
            if (roomId != room.Id)
            {
                return BadRequest();
            }

            await _roomService.UpdateRoomAsync(room);
            return NoContent();
        }

        [HttpDelete("{roomId}")]
        public async Task<IActionResult> DeleteRoom(Guid roomId)
        {
            await _roomService.DeleteRoomAsync(roomId);
            return NoContent();
        }

        [HttpPost("{roomId}/join")]
        public async Task<IActionResult> JoinRoom(Guid roomId, [FromQuery] string userId, [FromQuery] string code)
        {
            try
            {
                await _roomService.JoinRoomAsync(userId, roomId, code);
                await _hubContext.Clients.Group(roomId.ToString()).SendAsync("UserJoined", userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{roomId}/leave")]
        public async Task<IActionResult> RemoveUserOutOfRoom(Guid roomId, [FromQuery] string userId)
        {
            await _roomService.RemoveUserOutOfRoomAsync(userId, roomId);
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("UserLeft", userId);
            return Ok();
        }

        [HttpPost("{roomId}/music/add")]
        public async Task<IActionResult> AddMusicToRoom(Guid roomId, [FromQuery] Guid musicId)
        {
            await _roomService.AddMusicToRoomAsync(musicId, roomId);
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("MusicAdded", musicId);
            return Ok();
        }

        [HttpPost("{roomId}/music/remove")]
        public async Task<IActionResult> RemoveMusicOutOfRoom(Guid roomId, [FromQuery] Guid musicId)
        {
            await _roomService.RemoveMusicOutOfRoomAsync(musicId, roomId);
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("MusicRemoved", musicId);
            return Ok();
        }
    }
}
