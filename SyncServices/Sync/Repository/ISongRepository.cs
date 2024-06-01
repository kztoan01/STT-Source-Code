using Sync.DTOs;
using System;
using System.Collections.Generic;

namespace Sync.Repository
{
    public interface ISongRepository
    {
        SongDTO AddSong(SongDTO songDTO);
        SongDTO UpdateSong(SongDTO songDTO);
        void DeleteSong(Guid songId);
        SongDTO GetSongById(Guid songId);
        List<SongDTO> GetAllSongs();

        AlbumDTO AddAlbum(AlbumDTO albumDTO);
        AlbumDTO UpdateAlbum(AlbumDTO albumDTO);
        void DeleteAlbum(Guid albumId);
        AlbumDTO GetAlbumById(Guid albumId);
        List<AlbumDTO> GetAllAlbums();

        SongGenreDTO AddSongGenre(SongGenreDTO songGenreDTO);
        void DeleteSongGenre(Guid songGenreId);

        void AddSongToAlbum(Guid albumId, SongDTO songDTO);
        public void RemoveSongFromAlbum(Guid albumId, Guid songId);

        public void ChangeSongGenre(Guid songId, int newGenreId);
    }
}
