using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Dtos.Album
{
    public class CreateAlbumDTO
    {
        public string albumTitle {  get; set; }
        public string albumDescription { get; set; }
        public DateTime releaseDate { get; set; }
    }
}
