using Microsoft.EntityFrameworkCore;
using Sync.DTOs;
using Sync.Model;
using Sync.Repository;

namespace Sync.Services.iml
{
    public class LoginService : ILoginService
    {
        private readonly EFDataContext _context;

        private IUserRepository userRepository;

        public LoginService(EFDataContext context)
        {
            userRepository = new UserRepository(context);
            _context = context;
        }

        public UserDTO Login(string username, string password)
        {
            return userRepository.Login(username, password);
        }

        public UserDTO Register(UserDTO userDTO)
        {
            return userRepository.Register(userDTO);
        }
    }
}
