using sync_service.Dtos.Artist;
using sync_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Service.Interfaces
{
    public interface IArtistService
    {
        Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
        Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId);

    }
}