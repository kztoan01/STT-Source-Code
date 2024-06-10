using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Dtos.Account
{
    public record UserSession(string? Id, string? Name, string? Email, string? Role);
}