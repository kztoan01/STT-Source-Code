using Microsoft.EntityFrameworkCore;

namespace Sync.Model
{
    public class EFDataContext : DbContext
    {
       
            public EFDataContext(DbContextOptions<EFDataContext> options) : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Country> Countries { get; set; }
            public DbSet<Follower> Followers { get; set; }
            public DbSet<UserPlaylist> UserPlaylists { get; set; }
            public DbSet<Playlist> Playlists { get; set; }
            public DbSet<Artist> Artists { get; set; }
            public DbSet<Album> Albums { get; set; }
            public DbSet<Song> Songs { get; set; }
            public DbSet<Genre> Genres { get; set; }
            public DbSet<SongGenre> SongGenres { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Thiết lập các quan hệ nếu cần thiết
                modelBuilder.Entity<User>()
                    .HasMany(u => u.Followers)
                    .WithOne(f => f.User)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<User>()
                    .HasMany(u => u.UserPlaylists)
                    .WithOne(up => up.User)
                    .HasForeignKey(up => up.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Artist>()
                    .HasOne(a => a.User)
                    .WithMany()
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Album>()
                    .HasOne(a => a.Artist)
                    .WithMany()
                    .HasForeignKey(a => a.ArtistId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Song>()
                    .HasOne(s => s.Album)
                    .WithMany()
                    .HasForeignKey(s => s.AlbumId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<SongGenre>()
                    .HasOne(sg => sg.Song)
                    .WithMany(s => s.SongGenres)
                    .HasForeignKey(sg => sg.SongID)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<SongGenre>()
                    .HasOne(sg => sg.Genre)
                    .WithMany(g => g.SongGenres)
                    .HasForeignKey(sg => sg.GenreID)
                    .OnDelete(DeleteBehavior.NoAction);
                modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Country A" },
            new Country { Id = 2, Name = "Country B" }
        );

                // Seeding data cho bảng User
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "wiramin",
                        Password = "1234",
                        Name = "Minh Beo",
                        Birthdate = new DateTime(2012, 6, 18, 10, 34, 9),
                        Address = "hell hell",
                        City = "hcm",
                        Status = "active",
                        CountryId = 1,
                        Role = "Admin"
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "john_doe",
                        Password = "password",
                        Name = "John Doe",
                        Birthdate = new DateTime(1990, 1, 1),
                        Address = "123 Main St",
                        City = "New York",
                        Status = "inactive",
                        CountryId = 2,
                        Role = "User"
                    }
                );
            }
        }
    }

    
