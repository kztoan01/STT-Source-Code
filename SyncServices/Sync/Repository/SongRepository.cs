using Sync.DTOs;
using Sync.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sync.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly EFDataContext _context;

        public SongRepository(EFDataContext context)
        {
            _context = context;
        }

        public SongDTO AddSong(SongDTO songDTO)
        {
            var newSong = new Song
            {
                Id = Guid.NewGuid(),
                SongName = songDTO.SongName,
                SongDate = songDTO.SongDate,
                AlbumId = songDTO.AlbumId,
                IsApproved = false
            };

            _context.Songs.Add(newSong);
            _context.SaveChanges();

            songDTO.Id = newSong.Id;
            return songDTO;
        }

        public SongDTO UpdateSong(SongDTO songDTO)
        {
            var existingSong = _context.Songs.FirstOrDefault(s => s.Id == songDTO.Id);
            if (existingSong == null) return null;

            existingSong.SongName = songDTO.SongName;
            existingSong.SongDate = songDTO.SongDate;
            existingSong.AlbumId = songDTO.AlbumId;

            _context.SaveChanges();
            return songDTO;
        }

        public void DeleteSong(Guid songId)
        {
            var song = _context.Songs.FirstOrDefault(s => s.Id == songId);
            if (song != null)
            {
                _context.Songs.Remove(song);
                _context.SaveChanges();
            }
        }

        public SongDTO GetSongById(Guid songId)
        {
            var song = _context.Songs.FirstOrDefault(s => s.Id == songId);
            if (song == null) return null;

            return new SongDTO
            {
                Id = song.Id,
                SongName = song.SongName,
                SongDate = song.SongDate,
                AlbumId = song.AlbumId
            };
        }

        public List<SongDTO> GetAllSongs()
        {
            return _context.Songs.Select(s => new SongDTO
            {
                Id = s.Id,
                SongName = s.SongName,
                SongDate = s.SongDate,
                AlbumId = s.AlbumId
            }).ToList();
        }

        public AlbumDTO AddAlbum(AlbumDTO albumDTO)
        {
            var newAlbum = new Album
            {
                Id = Guid.NewGuid(),
                Name = albumDTO.Name,
                ReleaseDate = albumDTO.ReleaseDate,
                ArtistId = albumDTO.ArtistId
            };

            _context.Albums.Add(newAlbum);
            _context.SaveChanges();

            albumDTO.Id = newAlbum.Id;
            return albumDTO;
        }

        public AlbumDTO UpdateAlbum(AlbumDTO albumDTO)
        {
            var existingAlbum = _context.Albums.FirstOrDefault(a => a.Id == albumDTO.Id);
            if (existingAlbum == null) return null;

            existingAlbum.Name = albumDTO.Name;
            existingAlbum.ReleaseDate = albumDTO.ReleaseDate;
            existingAlbum.ArtistId = albumDTO.ArtistId;

            _context.SaveChanges();
            return albumDTO;
        }

        public void DeleteAlbum(Guid albumId)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == albumId);
            if (album != null)
            {
                _context.Albums.Remove(album);
                _context.SaveChanges();
            }
        }

        public AlbumDTO GetAlbumById(Guid albumId)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == albumId);
            if (album == null) return null;

            return new AlbumDTO
            {
                Id = album.Id,
                Name = album.Name,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId
            };
        }

        public List<AlbumDTO> GetAllAlbums()
        {
            return _context.Albums.Select(a => new AlbumDTO
            {
                Id = a.Id,
                Name = a.Name,
                ReleaseDate = a.ReleaseDate,
                ArtistId = a.ArtistId
            }).ToList();
        }

        public SongGenreDTO AddSongGenre(SongGenreDTO songGenreDTO)
        {
            var newSongGenre = new SongGenre
            {
                Id = Guid.NewGuid(),
                SongID = songGenreDTO.SongID,
                GenreID = songGenreDTO.GenreID
            };

            _context.SongGenres.Add(newSongGenre);
            _context.SaveChanges();

            songGenreDTO.Id = newSongGenre.Id;
            return songGenreDTO;
        }

        public void DeleteSongGenre(Guid songGenreId)
        {
            var songGenre = _context.SongGenres.FirstOrDefault(sg => sg.Id == songGenreId);
            if (songGenre != null)
            {
                _context.SongGenres.Remove(songGenre);
                _context.SaveChanges();
            }
        }

        public void AddSongToAlbum(Guid albumId, SongDTO songDTO)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == albumId);
            if (album == null)
            {
                throw new ArgumentException("Album not found");
            }

            var newSong = new Song
            {
                Id = Guid.NewGuid(),
                SongName = songDTO.SongName,
                SongDate = songDTO.SongDate,
                AlbumId = albumId
            };

            _context.Songs.Add(newSong);
            _context.SaveChanges();

            songDTO.Id = newSong.Id;
        }

        public void RemoveSongFromAlbum(Guid albumId, Guid songId)
        {
            var song = _context.Songs.FirstOrDefault(s => s.Id == songId && s.AlbumId == albumId);
            if (song != null)
            {
                _context.Songs.Remove(song);
                _context.SaveChanges();
            }
        }

        public void ChangeSongGenre(Guid songId, int newGenreId)
        {
            var songGenre = _context.SongGenres.FirstOrDefault(sg => sg.SongID == songId);
            if (songGenre != null)
            {
                songGenre.GenreID = newGenreId;
                _context.SaveChanges();
            }
        }

        public CheckResponseDTO CheckSong(Guid songId, int check)
        {
            var song = _context.Songs.FirstOrDefault(s => s.Id == songId);
            if (song == null)
            {
                return new CheckResponseDTO
                {
                    IsSuccess = false,
                    Message = "Song not found."
                };
            }

            if (check == 1)
            {
                song.IsApproved = true;
                _context.SaveChanges();
                return new CheckResponseDTO
                {
                    IsSuccess = true,
                    Message = "Song approved successfully."
                };
            }
            else if (check == 0)
            {
                string issueMessage = "The song content did not meet the approval criteria.";

                return new CheckResponseDTO
                {
                    IsSuccess = false,
                    Message = issueMessage
                };
            }
            else
            {
                return new CheckResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid check value."
                };
            }
        }

    }
}
