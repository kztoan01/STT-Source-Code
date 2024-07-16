using core.Dtos.User;
using core.Models;

namespace repository.Repository.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserIncludeArtist(Guid userId);
    Task<User> GetUserById(string userId);

    Task<List<UserDTO>> GetAllUser();
}