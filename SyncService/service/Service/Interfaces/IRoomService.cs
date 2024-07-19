using core.Models;

namespace service.Service.Interfaces;

public interface IRoomService
{
    Task<Room> GetRoomByIdAsync(Guid roomId);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room> GetRoomByUserIdAsync(string hostId);
    Task AddRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);

    Task<Participant> GetUserInRoomsAsync(Guid roomId, string userId);
    Task DeleteRoomAsync(Guid roomId);
    Task<Room> JoinRoomAsync(string userId, string code);
    Task RemoveUserOutOfRoomAsync(string userId, Guid roomId);
    Task<bool> AddMusicToRoomAsync(Guid musicId, Guid roomId);
    Task RemoveMusicOutOfRoomAsync(Guid musicId, Guid roomId);
}