using core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserArtistAsync(Guid userId); 
    }
}
