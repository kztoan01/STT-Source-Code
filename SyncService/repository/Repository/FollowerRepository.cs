using core.Models;
using data.Data;
using Microsoft.EntityFrameworkCore;
using repository.Repository.Interfaces;

namespace repository.Repository;

public class FollowerRepository:IFollowerRepository
{
    private readonly ApplicationDBContext _context;

    public FollowerRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    
    public async Task<Follower> AddFollower(Guid userId, Guid artistId)
    {
        Follower follower = new Follower()
        {
            artistId = artistId,
            userId = userId.ToString(),
            beginDate = DateTime.Now
        };
        await _context.AddAsync(follower);
        return follower;
    }

    public async Task<Follower> DeleteFollower(Guid userId, Guid artistId)
    {
        Follower follower =(await _context.Follower.Where(f => f.userId.Equals(userId.ToString()) && f.artistId == artistId)
            .FirstOrDefaultAsync())!;
        _context.Follower.Remove(follower);
        return follower;
    }
}