using core.Dtos.User;
using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserIncludeArtist(Guid userId)
        {
           return await _context.Users
                .Include(u => u.Artist)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users
                 .FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }

        public async Task<List<UserDTO>> GetAllUser()
        {
            var tmp = await _context.Users
                .Include(p=>p.Artist)
                .Include(p=>p.MusicHistories)
                .Include(p=>p.Followers)
                .ToListAsync();
            var result =  new List<UserDTO>();
            foreach (var VARIABLE in tmp)
            {
                result.Add(new UserDTO(VARIABLE));
            }
            return result;
        }
}
