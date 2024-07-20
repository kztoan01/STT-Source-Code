using core.Models;

namespace service.Service.Interfaces;

public interface IUserService
{
    Task<User> GetUserArtistAsync(Guid userId);
    Task<User> GetUserByIdAsync(string userId);
    Task<Follower> FollowArtist(Guid userId, Guid artistId);
    Task<Follower> UnFollowArtist(Guid userId, Guid artisId);
}