using sync_service.Dtos.Album;
using sync_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Interfaces
{
    public interface IAlbumRepository
    {
        Task <List<Album>> getAlbumByGenreNameAsync (string genreName);
        Task<List<Album>> getAllAlbumsAsync();
        Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId);

    }
}