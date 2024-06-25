using core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserIncludeArtist(Guid userId);
    }
}
