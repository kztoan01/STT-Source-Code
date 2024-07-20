using core.Models;

namespace repository.Repository.Interfaces;

public interface IRoomRepository
{
    Task<Room> GetRoomByIdAsync(Guid roomId);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task AddRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task<Participant> GetUserInRoomsAsync(Guid roomId, string userId);
    Task<Room> GetRoomByUserIdAsync(string hostId);
    Task DeleteRoomAsync(Guid roomId);
    Task<Room> JoinRoomAsync(string userId, string code);
    Task RemoveUserOutOfRoomAsync(string userId, Guid roomId);
    Task<bool> AddMusicToRoomAsync(Guid musicId, Guid roomId);

    Task RemoveMusicOutOfRoomAsync(Guid musicId, Guid roomId);
}