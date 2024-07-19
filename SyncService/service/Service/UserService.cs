using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IFollowerRepository _followerRepository;

    public UserService(IUserRepository userRepository, IFollowerRepository followerRepository)
    {
        _followerRepository = followerRepository;
        _userRepo = userRepository;
    }

    public async Task<User> GetUserArtistAsync(Guid userId)
    {
        var user = await _userRepo.GetUserIncludeArtist(userId);
        if (user == null) return null;
        return user;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _userRepo.GetUserById(userId);
        if (user == null) return null;
        return user;
    }

    public async Task<Follower> FollowArtist(Guid userId, Guid artistId)
    {
        return await _followerRepository.AddFollower(userId, artistId);
    }

    public async Task<Follower> UnFollowArtist(Guid userId, Guid artisId)
    {
        return await _followerRepository.DeleteFollower(userId, artisId);
    }
}