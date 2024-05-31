using Sync.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sync.DTOs
{
    public class UserDTO
    {

        public Guid Id { get; set; } 

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public string Address { get; set; }


        public string City { get; set; }

        public string Status { get; set; }

        public string Role { get; set; }

        public int CountryId { get; set; }

        public String CountryName { get; set; }

        public virtual int NumberOfFollowers { get; set; }
        public virtual int NumberOfPlaylists{ get; set; }
    }
}
