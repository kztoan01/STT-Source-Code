﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Models
{
    public class Participant
    {

        public User User { get; set; }

        public string UserId { get; set; }

        public Room Room { get; set; }

        public Guid RoomId { get; set; }

        public DateTime JoinTime { get; set; }


    }
}
