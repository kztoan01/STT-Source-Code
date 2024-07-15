using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Dtos.Room
{
    public class RoomPlaylistDto
    {
        public Guid MusicId { get; set; }
        public string MusicName { get; set; }
        public DateTime AddTime { get; set; }
    }

}
