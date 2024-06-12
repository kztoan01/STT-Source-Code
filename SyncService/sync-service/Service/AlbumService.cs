using sync_service.Interfaces;
using sync_service.Models;
using sync_service.Service.Interfaces;

namespace sync_service.Service
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }
        public async Task<List<Album>> getAlbumByGenreNameAsync(string genreName)
        {
            return await _albumRepository.getAlbumByGenreNameAsync(genreName);
        }
    }
}
