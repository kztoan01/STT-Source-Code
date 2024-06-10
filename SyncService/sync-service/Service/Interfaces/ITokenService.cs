using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Account;
using sync_service.Models;

namespace sync_service.Service.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        string GenerateToken(UserSession user);
    }
}