using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sync_music_service.Models;

namespace sync_music_service.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<PlaylistMusic> PlaylistMusics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaylistMusic>()
            .HasKey(pm => new { pm.playlistId, pm.musicId });

        modelBuilder.Entity<PlaylistMusic>()
            .HasOne(pm => pm.Playlist)
            .WithMany(p => p.playlistMusics)
            .HasForeignKey(pm => pm.playlistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistMusic>()
            .HasOne(pm => pm.Music)
            .WithMany(m => m.playlistMusics)
            .HasForeignKey(pm => pm.musicId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Music>()
            .HasOne(m => m.Album)
            .WithMany(a => a.Musics)
            .HasForeignKey(m => m.albumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Music>()
            .HasOne(m => m.Artist)
            .WithMany(a => a.Musics)
            .HasForeignKey(m => m.artistId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Music>()
            .HasOne(m => m.Genre)
            .WithMany(g => g.Musics)
            .HasForeignKey(m => m.genreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    }
}