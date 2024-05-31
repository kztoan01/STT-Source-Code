using Sync.DTOs;

namespace Sync.Services
{
    public interface IUserService
    {

        public UserDTO getById(Guid id);

        public UserDTO Delete(UserDTO userDTO);

        public List<UserDTO> getAllUsers();

        public UserDTO getByName(string name);

        public UserDTO Update(UserDTO userDTO);

        public UserDTO DeactivateUser(Guid userId);

    }
}
