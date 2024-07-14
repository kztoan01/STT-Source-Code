using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Dtos.Artist
{
    public class ArtistImageDTO
    {
        public Guid id { get; set; }
        public IFormFile image { get; set; }
    }
}
