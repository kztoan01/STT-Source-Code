using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Models
{
    public class RoomPlaylist
    {
        public Room Room { get; set; }

        public Music Music { get; set; }

        public DateTime AddTime { get; set; }

        public Guid MusicId { get; set; }

        public Guid RoomId { get; set; }


    }
}
