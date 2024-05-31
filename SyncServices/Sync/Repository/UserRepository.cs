using Sync.Model;
using Sync.DTOs;
using System.Linq;
using System;

namespace Sync.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EFDataContext _context;

        public UserRepository(EFDataContext context)
        {
            _context = context;
        }

        public UserDTO Delete(UserDTO userDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userDTO.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return userDTO;
        }

        public List<UserDTO> getAllUsers()
        {
            var users = _context.Users.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Address = user.Address,
                City = user.City,
                Status = user.Status,
                CountryName = user.Country.Name,
                NumberOfFollowers = user.Followers.Count,
                NumberOfPlaylists = user.UserPlaylists.Count
            }).ToList();

            return users;
        }

        public UserDTO getById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Address = user.Address,
                City = user.City,
                Status = user.Status,
                CountryName = user.Country.Name,
                NumberOfFollowers = user.Followers.Count,
                NumberOfPlaylists = user.UserPlaylists.Count
            };
        }


        public UserDTO getByName(string name)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == name);
            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Address = user.Address,
                City = user.City,
                Status = user.Status,
                CountryName = user.Country.Name,
                NumberOfFollowers = user.Followers.Count,
                NumberOfPlaylists = user.UserPlaylists.Count
            };
        }

        public UserDTO Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Address = user.Address,
                City = user.City,
                Status = user.Status,
                CountryName = user.Country.Name,
                NumberOfFollowers = user.Followers.Count,
                NumberOfPlaylists = user.UserPlaylists.Count
            };
        }

        public UserDTO Register(UserDTO userDTO)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == userDTO.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists.");
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = userDTO.Username,
                Password = userDTO.Password,
                Name = userDTO.Name,
                Birthdate = userDTO.Birthdate,
                Address = userDTO.Address,
                City = userDTO.City,
                Status = "active",
                CountryId = userDTO.CountryId
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new UserDTO
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Password = newUser.Password,
                Name = newUser.Name,
                Birthdate = newUser.Birthdate,
                Address = newUser.Address,
                City = newUser.City,
                Status = newUser.Status,
                CountryName = _context.Countries.FirstOrDefault(c => c.Id == newUser.CountryId)?.Name,
                NumberOfFollowers = 0,
                NumberOfPlaylists = 0
            };
        }

        public UserDTO Update(UserDTO userDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userDTO.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.Username = userDTO.Username;
            user.Password = userDTO.Password;
            user.Name = userDTO.Name;
            user.Birthdate = userDTO.Birthdate;
            user.Address = userDTO.Address;
            user.City = userDTO.City;
            user.Status = userDTO.Status;
            user.CountryId = userDTO.CountryId;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Address = user.Address,
                City = user.City,
                Status = user.Status,
                CountryName = _context.Countries.FirstOrDefault(c => c.Id == user.CountryId)?.Name,
                NumberOfFollowers = user.Followers.Count,
                NumberOfPlaylists = user.UserPlaylists.Count
            };
        }

        public UserDTO DeactivateUser(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.Status = "inactive";
            _context.Users.Update(user);
            _context.SaveChanges();

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Address = user.Address,
                City = user.City,
                Status = user.Status,
                CountryName = user.Country.Name,
                NumberOfFollowers = user.Followers.Count,
                NumberOfPlaylists = user.UserPlaylists.Count
            };
        }

        public UserDTO ChangeUserRole(Guid userId, int role)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            switch (role)
            {
                case 1:
                    user.Role = "admin";
                    break;
                case 2:
                    user.Role = "staff";
                    break;
                case 3:
                    user.Role = "user";
                    break;
                default:
                    throw new ArgumentException("Invalid role value. Accepted values are 1, 2, or 3.");
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Address = user.Address,
                City = user.City,
                Status = user.Status,
                Role = user.Role,
                CountryName = user.Country.Name,
                NumberOfFollowers = user.Followers.Count,
                NumberOfPlaylists = user.UserPlaylists.Count
            };
        }
    }
}
