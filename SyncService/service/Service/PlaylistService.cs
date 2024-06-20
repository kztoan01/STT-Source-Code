using core.Dtos.Album;
using core.Dtos.Playlist;
using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class PlaylistService : IPlaylistService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistService(IPlaylistRepository playlistRepository, IAlbumRepository albumRepository)
    {
        _playlistRepository = playlistRepository;
        _albumRepository = albumRepository;
    }

    public async Task<Playlist> CreatePlaylistAsync(Playlist playlist)
    {
        return await _playlistRepository.CreatePlaylistAsync(playlist);
    }

    public async Task<Playlist?> DeletePlaylistAsync(Guid id)
    {
        return await _playlistRepository.DeletePlaylistAsync(id);
    }

    public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
    {
        return await _playlistRepository.GetPlaylistByIdAsync(id);
    }

    public async Task<List<Playlist>> GetPlaylistsByGenreNameAsync(string genreName)
    {
        return await _playlistRepository.GetPlaylistsByGenreNameAsync(genreName);
    }

    public async Task<List<PlaylistDTO>> GetUserPlaylistAsync(string userId)
    {
        return await _playlistRepository.GetUserPlaylistAsync(userId);
    }

    public async Task<List<Playlist>> ShowPlaylistByUserId(Guid UserId)
    {
        return await _playlistRepository.ShowPlaylistByUserId(UserId);
    }

    public async Task<Playlist?> UpdatePlaylistAsync(Guid id, Playlist playlistModel)
    {
        return await _playlistRepository.UpdatePlaylistAsync(id, playlistModel);
    }

    public async Task<string> AddMusicIntoPlaylist(Guid musicId, Guid playlistId)
    {
        return await _playlistRepository.AddMusicIntoPlaylist(musicId, playlistId);
    }

    public async Task<string> AddEntireAlbumIntoPlaylist(Guid albumId, Guid playlistId)
    {
        return await _playlistRepository.AddEntireAlbumIntoPlaylist(albumId, playlistId);
    }

    public async Task<List<AlbumDTO>> GetAlbumByContainArtistByArtistId(Guid artistId)
    {
        return await _albumRepository.GetAlbumByContainArtistByArtistId(artistId);
    }

    public async Task<string> DeleteAMusicInPlaylist(Guid musicId, Guid playlistId)
    {
        return await _playlistRepository.DeleteAMusicInPlaylist(musicId, playlistId);
    }

    public async Task<string> ChangeMusicPositionInPlaylist(Guid musicId1, int newPosition, Guid playlistId)
    {
        return await _playlistRepository.ChangeMusicPositionInPlaylist(musicId1, newPosition, playlistId);
    }
}