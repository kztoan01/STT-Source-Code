using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Dtos.Room
{
    public class ParticipantDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime JoinTime { get; set; }
    }
}
