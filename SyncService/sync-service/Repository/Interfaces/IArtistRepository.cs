using sync_service.Service;
using sync_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sync_service.Dtos.Artist;

namespace sync_service.Interfaces
{
    public interface IArtistRepository
    {
        Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId);
        Task<List<AlbumResponseDTO>> GetAllArtistAlbumsAsync(Guid artistId);

    }
}