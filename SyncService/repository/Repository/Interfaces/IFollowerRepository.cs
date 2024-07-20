using core.Models;

namespace repository.Repository.Interfaces;

public interface IFollowerRepository
{
    Task<Follower> AddFollower(Guid userId, Guid artistId);
    Task<Follower> DeleteFollower(Guid userId, Guid artistId);
}