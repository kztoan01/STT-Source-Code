using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Dtos.Album;
using sync_service.Dtos.Artist;
using sync_service.Interfaces;
using sync_service.Models;
using System.Collections.Generic;

namespace sync_service.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationDBContext _context;

        public AlbumRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId)
        {
            var albums = await _context.Albums
                .Where(a => a.artistId == artistId)
                .ToListAsync();

 
            var albumDTOs = albums.Select(album => new AlbumDTO
            {
                Id = album.Id,
                albumTitle = album.albumTitle,
                releaseDate = album.releaseDate,
                albumDescription = album.albumDescription,
            }).ToList();

            return albumDTOs;
        }

        public async Task<List<Album>> getAlbumByGenreNameAsync(string genreName)
        {
            // handle mapper later
            return await _context.Albums
                .Include(a =>a.Musics)
                .Where(a => a.Musics
                .Any(m => m.Genre.genreName == genreName))
                .ToListAsync();
        }

        public async Task<AlbumResponseDTO> GetAlbumDetails(Guid albumId)
        {
              var album = await _context.Albums
                .Include(a => a.Musics)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            var artistId = album.artistId;

            var artist = await _context.Artists
                        .FirstOrDefaultAsync(a => a.Id == artistId);
            var albumDTO = new AlbumResponseDTO
            {
                Id = album.Id,
                albumTitle = album.albumTitle,
                releaseDate = album.releaseDate,
                albumDescription = album.albumDescription
            };
            return albumDTO;
        }

        public async Task<List<Album>> getAllAlbumsAsync()
        {
            return await _context.Albums
                .Include (a => a.Musics)
                .ToListAsync();
        }

        public async Task<Album> GetMostListenAlbum()
        {

            var albumList = await _context.Albums.Include(a => a.Musics).ToListAsync();

            if (albumList == null )
            {
                return null;
            }

            var mostListenAlbum = albumList
                .Select(album => new
                {
                    Album = album,
                    TotalPlays = album.Musics.Sum(music => music.musicPlays)
                })
                .OrderByDescending(albumWithPlays => albumWithPlays.TotalPlays)
                .FirstOrDefault();

            // Return the album with the most plays
            return mostListenAlbum?.Album;
        }
    }
}
