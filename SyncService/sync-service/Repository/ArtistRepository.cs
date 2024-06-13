﻿using Microsoft.EntityFrameworkCore;
using sync_service.Data;
using sync_service.Dtos.Album;
using sync_service.Dtos.Artist;
using sync_service.Dtos.Music;
using sync_service.Interfaces;
using sync_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sync_service.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDBContext _context;

        public ArtistRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ArtistDTO> GetArtistDTOById(Guid id)
        {
            var artist = await _context.Artists
                .Include(a => a.User)
                .Include(a => a.Musics)
                    .ThenInclude(m => m.Genre)
                .Include(a => a.Musics)
                    .ThenInclude(m => m.Album)
                .Include(a => a.Musics)
                    .ThenInclude(m => m.playlistMusics)
                .Include(a => a.Albums)
                .Include(a => a.Followers)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return null;
            }

            var artistDTO = new ArtistDTO
            {
                Id = artist.Id,
                userId = artist.userId,
                AristName = artist.User.UserName,
                artistDescription = artist.artistDescription,
                NumberOfFollower = artist.Followers.Count,
                Albums = artist.Albums.Select(a => new AlbumDTO
                {
                    Id = a.Id,
                    albumTitle = a.albumTitle
                }).ToList(),
                ViralMusics = artist.Musics.Select(m => new MusicDTO
                {
                    Id = m.Id,
                    genreName = m.Genre.genreName,
                    musicDuration = m.musicDuration,
                    musicPicture = m.musicPicture,
                    musicPlays = m.musicPlays,
                    musicTitle = m.musicTitle,
                    musicUrl = m.musicUrl,
                    releaseDate = m.releaseDate
                }).ToList()
            };

            return artistDTO;
        }
    }
}
