﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Models
{
    public class MusicHistory
    {
        public Guid Id { get; set; }
        public Guid MusicId { get; set; }
        public Music Music { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime ListenTime { get; set; }
               
    }
}