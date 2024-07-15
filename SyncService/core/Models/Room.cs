using core.Dtos.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace core.Models
{
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public User User { get; set; }

        public string Code { get; set; }    

        public string Image {  get; set; }
        public string HostId { get; set; }

        public List<RoomPlaylist> RoomPlaylists { get; set; }

        public List<Participant> Participants { get; set; }
    }
}
