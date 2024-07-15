using core.Dtos.Room;
using core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using service.Hub;
using service.Hub.iml;
using service.Service.Interfaces;

namespace controller.Controllers;

[Route("room-service/api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IHubContext<RoomHub, IRoomHub> _hubContext;
    private readonly IMusicService _musicService;
    private readonly IRoomService _roomService;
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;

    public RoomController(IMusicService musicService, IRoomService roomService,
        IHubContext<RoomHub, IRoomHub> hubContext, IUserService userService, UserManager<User> userManager)
    {
        _roomService = roomService;
        _hubContext = hubContext;
        _userService = userService;
        _userManager = userManager;
        _musicService = musicService;
    }

    [HttpGet("{roomId}")]
    [Authorize]
    public async Task<IActionResult> GetRoomById(Guid roomId)
    {
        var room = await _roomService.GetRoomByIdAsync(roomId);
        if (room == null) return NotFound();
        var roomDto = new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Code = room.Code,
            Image = room.Image,
            HostId = room.HostId,
            Participants = room.Participants
                .Select(p => new ParticipantDto { UserId = p.UserId, UserName = p.User.UserName }).ToList(),
            RoomPlaylists = room.RoomPlaylists.Select(rp => new RoomPlaylistDto
                { MusicId = rp.MusicId, MusicName = rp.Music.musicTitle }).ToList()
        };

        return Ok(roomDto);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        return Ok(rooms);
    }

    [HttpPost("AddRoom")]
    [Authorize]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomDTO addRoomDTO)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound("User not found");
        var room = new Room
        {
            Code = GenerateUniqueCode(),
            Image = addRoomDTO.Image,
            Name = addRoomDTO.Name,
            HostId = user.Id
        };

        await _roomService.AddRoomAsync(room);
        return CreatedAtAction(nameof(GetRoomById), new { roomId = room.Id }, room);
    }

    private string GenerateUniqueCode()
    {
        return Guid.NewGuid().ToString().Substring(0, 6);
    }


    [HttpPut("{roomId}")]
    [Authorize]
    public async Task<IActionResult> UpdateRoom(Guid roomId, [FromBody] Room room)
    {
        if (roomId != room.Id) return BadRequest();

        await _roomService.UpdateRoomAsync(room);
        return NoContent();
    }

    [HttpDelete("{roomId}")]
    public async Task<IActionResult> DeleteRoom(Guid roomId)
    {
        await _roomService.DeleteRoomAsync(roomId);
        return NoContent();
    }

    [HttpPost("join")]
    [Authorize]
    public async Task<IActionResult> JoinRoom([FromBody] JoinRoomDTO joinRoomDTO)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found");
            await _roomService.JoinRoomAsync(user.Id, Guid.Parse(joinRoomDTO.roomId), joinRoomDTO.code);
            await _hubContext.Clients.Group(joinRoomDTO.roomId).AlertToRoom(joinRoomDTO.roomId, user.UserName);
            await _hubContext.Clients.Group(joinRoomDTO.roomId).UpdateParticipantsList();
            var room = await _roomService.GetRoomByIdAsync(Guid.Parse(joinRoomDTO.roomId));
            return Ok(room);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("leave")]
    [Authorize]
    public async Task<IActionResult> RemoveUserOutOfRoom([FromBody] LeaveRoomDTO leaveRoomDTO)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound("User not found");
        await _roomService.RemoveUserOutOfRoomAsync(user.Id, Guid.Parse(leaveRoomDTO.roomId));
        await _hubContext.Clients.Group(leaveRoomDTO.roomId).OnLeaveRoom(leaveRoomDTO.roomId, user.UserName);
        await _hubContext.Clients.Group(leaveRoomDTO.roomId).UpdateParticipantsList();
        return Ok();
    }

    [HttpPost("music/add")]
    [Authorize]
    public async Task<IActionResult> AddMusicToRoom(AddRoomMusicDTO addRoomMusicDTO)
    {
        var music = await _musicService.GetMusicByMusicIdAsync(Guid.Parse(addRoomMusicDTO.musicId));
        await _roomService.AddMusicToRoomAsync(music.Id, Guid.Parse(addRoomMusicDTO.roomId));
        await _hubContext.Clients.Group(addRoomMusicDTO.roomId)
            .OnAddRoomMusic(addRoomMusicDTO.roomId, music.musicTitle);
        await _hubContext.Clients.Group(addRoomMusicDTO.roomId).UpdateMusicsList();
        return Ok();
    }

    [HttpPost("music/remove")]
    [Authorize]
    public async Task<IActionResult> RemoveMusicOutOfRoom(RemoveRoomMusicDTO removeRoomMusicDTO)
    {
        var music = await _musicService.GetMusicByMusicIdAsync(Guid.Parse(removeRoomMusicDTO.musicId));
        await _roomService.RemoveMusicOutOfRoomAsync(music.Id, Guid.Parse(removeRoomMusicDTO.roomId));
        await _hubContext.Clients.Group(removeRoomMusicDTO.roomId)
            .OnRemoveRoomMusic(removeRoomMusicDTO.roomId, music.musicTitle);
        await _hubContext.Clients.Group(removeRoomMusicDTO.roomId).UpdateMusicsList();
        return Ok();
    }
}