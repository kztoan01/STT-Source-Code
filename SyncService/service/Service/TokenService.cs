using core.Dtos.Account;
using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class TokenService : ITokenService
{
    private readonly ITokenRepository _tokenRepository;

    public TokenService(ITokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    public string CreateToken(User user)
    {
        return _tokenRepository.CreateToken(user);
    }

    public string GenerateToken(UserSession user)
    {
        return _tokenRepository.GenerateToken(user);
    }
}