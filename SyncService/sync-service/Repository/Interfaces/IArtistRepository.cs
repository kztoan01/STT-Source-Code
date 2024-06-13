using sync_service.Dtos.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Interfaces
{
    public interface IArtistRepository
    {
        Task<ArtistDTO> GetArtistDTOById(Guid id);
    }
}