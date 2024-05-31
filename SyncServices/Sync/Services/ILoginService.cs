using Sync.DTOs;
using Sync.Model;

namespace Sync.Services
{
    public interface ILoginService
    {
        public UserDTO Register(UserDTO userDTO);

        public UserDTO Login(string username, string password);

    }
}
