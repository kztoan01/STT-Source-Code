using Sync.DTOs;

namespace Sync.Repository
{
    public interface IUserRepository
    {
        public UserDTO Login(string username, string password);

        public UserDTO Register(UserDTO userDTO);

        public UserDTO Update(UserDTO userDTO);

        public UserDTO Delete(UserDTO userDTO);

        public List<UserDTO> getAllUsers();

        public UserDTO getById(Guid id);

        public UserDTO getByName(string name);

        public UserDTO DeactivateUser(Guid userId);

    }
}
