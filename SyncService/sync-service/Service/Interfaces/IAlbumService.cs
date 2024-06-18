using sync_service.Dtos.Album;
using sync_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Service.Interfaces
{
    public interface IAlbumService
    {
        Task<List<Album>> getAlbumByGenreNameAsync (string genreName);
        Task<List<Album>> getAllAlbumsAsync();
        Task<AlbumResponseDTO> GetAlbumDetail(Guid albumId);
        Task<Album> GetMostListenAlbum();
    }
}