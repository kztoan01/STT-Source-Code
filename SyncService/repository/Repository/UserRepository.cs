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
                 .Include(u => u.MusicHistories)
                 .FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }
    }
