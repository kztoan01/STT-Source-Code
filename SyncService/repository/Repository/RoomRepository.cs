using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDBContext _context;

    public RoomRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Room> GetRoomByIdAsync(Guid roomId)
    {
        return await _context.Rooms
            .Include(r => r.RoomPlaylists)
            .ThenInclude(rp => rp.Music)
            .Include(r => r.Participants)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _context.Rooms
            .Include(r => r.User)
            .Include(r => r.RoomPlaylists).ThenInclude(rp => rp.Music)
            .Include(r => r.Participants).ThenInclude(p => p.User)
            .ToListAsync();
    }

    public async Task AddRoomAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoomAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRoomAsync(Guid roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room != null)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }

    public async Task JoinRoomAsync(string userId, Guid roomId, string code)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        if (room == null) throw new Exception("Room is not avaialble.");

        if (room.Code != code) throw new Exception("Invalid code.");

        var participant = new Participant
        {
            UserId = userId,
            RoomId = roomId
        };

        await _context.Participants.AddAsync(participant);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserOutOfRoomAsync(string userId, Guid roomId)
    {
        var participant = await _context.Participants
            .FirstOrDefaultAsync(p => p.UserId == userId && p.RoomId == roomId);

        if (participant != null)
        {
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddMusicToRoomAsync(Guid musicId, Guid roomId)
    {
        var roomPlaylist = new RoomPlaylist
        {
            MusicId = musicId,
            RoomId = roomId,
            AddTime = DateTime.UtcNow
        };

        await _context.RoomPlaylists.AddAsync(roomPlaylist);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveMusicOutOfRoomAsync(Guid musicId, Guid roomId)
    {
        var roomPlaylist = await _context.RoomPlaylists
            .FirstOrDefaultAsync(rp => rp.MusicId == musicId && rp.RoomId == roomId);

        if (roomPlaylist != null)
        {
            _context.RoomPlaylists.Remove(roomPlaylist);
            await _context.SaveChangesAsync();
        }
    }
}