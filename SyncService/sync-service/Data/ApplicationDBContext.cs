using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sync_service.Models;

namespace sync_service.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
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
        public DbSet<MusicListen> MusicListens { get; set; }

        public DbSet<Collaboration> Collaborations { get; set; }
        public DbSet<PlaylistMusic> PlaylistMusics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(c => c.Id);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.Entity<User>()
           .HasOne(u => u.Artist)
           .WithOne(a => a.User)
           .HasForeignKey<Artist>(a => a.userId);

            modelBuilder.Entity<PlaylistMusic>()
                .HasKey(pm => new { pm.playlistId, pm.musicId });

            modelBuilder.Entity<PlaylistMusic>()
                .HasOne(pm => pm.Playlist)
                .WithMany(p => p.playlistMusics)
                .HasForeignKey(pm => pm.playlistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MusicListen>()
                .HasOne(ml => ml.Music)
                .WithMany(m => m.MusicListens)
                .HasForeignKey(ml => ml.MusicId);

            modelBuilder.Entity<Collaboration>()
                .HasOne(ml => ml.Music)
                .WithMany(m => m.collaborations)
                .HasForeignKey(ml => ml.MusicId);

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

            modelBuilder.Entity<Follower>()
            .HasKey(f => new { f.userId, f.artistId });


            modelBuilder.Entity<Follower>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.userId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follower>()
                .HasOne(f => f.Artist)
                .WithMany(a => a.Followers)
                .HasForeignKey(f => f.artistId);


            List<IdentityRole> roles = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    },
                    new IdentityRole
                    {
                        Name = "Artist",
                        NormalizedName = "ARTIST"
                    },
                };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}