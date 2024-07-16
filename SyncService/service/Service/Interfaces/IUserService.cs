using core.Models;

namespace service.Service.Interfaces;

public interface IUserService
{
    Task<User> GetUserArtistAsync(Guid userId);
    Task<User> GetUserByIdAsync(string userId);
}