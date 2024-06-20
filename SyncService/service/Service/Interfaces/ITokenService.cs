using core.Dtos.Account;
using core.Models;

namespace service.Service.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
    string GenerateToken(UserSession user);
}