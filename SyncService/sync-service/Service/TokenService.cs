using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Account;
using sync_service.Models;
using sync_service.Repository.Interfaces;
using sync_service.Service.Interfaces;

namespace sync_service.Service
{
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
}