using Sync.DTOs;
using Sync.Model;
using Sync.Repository;

namespace Sync.Services.iml
{
    public class UserService : IUserService
    {

        private readonly EFDataContext _context;

        private IUserRepository userRepository;

        public UserService(EFDataContext context)
        {
            userRepository = new UserRepository(context);
            _context = context;
        }

        public UserDTO DeactivateUser(Guid userId)
        {
            return userRepository.DeactivateUser(userId);
        }

        public UserDTO Delete(UserDTO userDTO)
        {
            return userRepository.Delete(userDTO);
        }

        public List<UserDTO> getAllUsers()
        {
           return userRepository.getAllUsers();
        }

        public UserDTO getById(Guid id)
        {
            return userRepository.getById(id);
        }

        public UserDTO getByName(string name)
        {
            return userRepository.getByName(name);
        }

        public UserDTO Update(UserDTO userDTO)
        {
            return userRepository.Update(userDTO);
        }
    }
}
