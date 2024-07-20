using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Room> GetRoomByIdAsync(Guid roomId)
    {
        return await _roomRepository.GetRoomByIdAsync(roomId);
    }

    public async Task<Room> GetRoomByUserIdAsync(string hostId)
    {
        return await _roomRepository.GetRoomByUserIdAsync(hostId);
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllRoomsAsync();
    }

    public async Task<Participant> GetUserInRoomsAsync(Guid roomId,string userId)
    {
        return await _roomRepository.GetUserInRoomsAsync(roomId,userId);
    }


    public async Task AddRoomAsync(Room room)
    {
        await _roomRepository.AddRoomAsync(room);
    }

    public async Task UpdateRoomAsync(Room room)
    {
        await _roomRepository.UpdateRoomAsync(room);
    }

    public async Task DeleteRoomAsync(Guid roomId)
    {
        await _roomRepository.DeleteRoomAsync(roomId);
    }

    public async Task<Room> JoinRoomAsync(string userId, string code)
    {
       return await _roomRepository.JoinRoomAsync(userId, code);
    }

    public async Task RemoveUserOutOfRoomAsync(string userId, Guid roomId)
    {
        await _roomRepository.RemoveUserOutOfRoomAsync(userId, roomId);
    }

    public async Task<bool> AddMusicToRoomAsync(Guid musicId, Guid roomId)
    {
        return await _roomRepository.AddMusicToRoomAsync(musicId, roomId);
    }

    public async Task RemoveMusicOutOfRoomAsync(Guid musicId, Guid roomId)
    {
        await _roomRepository.RemoveMusicOutOfRoomAsync(musicId, roomId);
    }
}