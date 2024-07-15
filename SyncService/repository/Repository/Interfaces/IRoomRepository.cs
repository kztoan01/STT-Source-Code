using core.Models;

namespace repository.Repository.Interfaces;

public interface IRoomRepository
{
    Task<Room> GetRoomByIdAsync(Guid roomId);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task AddRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(Guid roomId);
    Task JoinRoomAsync(string userId, Guid roomId, string code);
    Task RemoveUserOutOfRoomAsync(string userId, Guid roomId);
    Task AddMusicToRoomAsync(Guid musicId, Guid roomId);

    Task RemoveMusicOutOfRoomAsync(Guid musicId, Guid roomId);
}