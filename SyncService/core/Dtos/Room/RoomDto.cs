using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Dtos.Room
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string HostId { get; set; }
        public List<RoomPlaylistDto> RoomPlaylists { get; set; }
        public List<ParticipantDto> Participants { get; set; }
    }

}
