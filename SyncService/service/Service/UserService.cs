using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService (IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<User> GetUserArtistAsync(Guid userId)
        {
            var user = await _userRepo.GetUserIncludeArtist(userId);
            if (user == null) {
                return null;
            }
            return user;
        }
    }
}
