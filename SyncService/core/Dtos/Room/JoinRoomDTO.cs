using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Dtos.Room
{
    public class JoinRoomDTO
    {
        public  string code {  get; set; }

        public string userId { get; set; }

        public string roomId { get; set; }
    }
}
