using core.Dtos.Account;
using core.Models;

namespace repository.Repository.Interfaces;

public interface ITokenRepository
{
    string CreateToken(User user);
    string GenerateToken(UserSession user);
}