using core.Dtos.Album;
using core.Dtos.Artist;
using core.Dtos.User;
using core.Models;
using core.Objects;
using core.Objects.Enum;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class AdminService(
    IArtistRepository artistRepository,
    IUserRepository userRepository,
    IAlbumRepository albumRepository,
    IPlaylistRepository playlistRepository,
    IMusicRepository musicRepository)
    : IAdminService
{
    public async Task<List<ArtistDTO>> getArtist(QueryArtist queryObject)
    {
        var page = queryObject.PageNumber;
        var pageSize = queryObject.PageSize;
        var isDesc = queryObject.IsDescending;
        var sortBy = queryObject.SortBy;
        var search = queryObject.SearchTerm;

        var list = await artistRepository.GetAllArtistDTOs();

        switch (sortBy)
        {
            case ArtistSearchByEnum.AristName:
                list = list.Where(a => a.AristName.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.AristName).ToList() : list.OrderBy(a => a.AristName).ToList();
                break;
            case ArtistSearchByEnum.ArtistDescription:
                list = list.Where(a => a.artistDescription.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.artistDescription).ToList() : list.OrderBy(a => a.artistDescription).ToList();
                break;
            case ArtistSearchByEnum.NumberOfFollower:
                list = list.Where(a => a.AristName.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.NumberOfFollower).ToList() : list.OrderBy(a => a.NumberOfFollower).ToList();
                break;
            default:
                list = list.Where(a => a.AristName.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.AristName).ToList() : list.OrderBy(a => a.AristName).ToList();
                break;
        }

        var paginatedList = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return paginatedList;
    }




    public async Task<List<UserDTO>> getUser(QueryUser queryObject)
    {
        var page = queryObject.PageNumber;
        var pagSize = queryObject.PageSize;
        var isDes = queryObject.IsDecsending;
        var sort = queryObject.SortBy;
        var search = queryObject.Name;
        var list = await userRepository.GetAllUser();
        if(sort == UserSearchByEnum.userFullName)
            list = list.Where(p => p.userFullName.Contains(search ?? "")).OrderBy(u => u.userFullName).ToList();
        else if(sort == UserSearchByEnum.birthday)
            list = list.Where(p => p.userFullName.Contains(search ?? "")).OrderBy(u => u.birthday).ToList();
        else if (sort == UserSearchByEnum.address)
            list = list.Where(p => p.userFullName.Contains(search ?? "")).OrderBy(u => u.address).ToList();
        else if(sort == UserSearchByEnum.city)
            list = list.Where(p => p.userFullName.Contains(search ?? "")).OrderBy(u => u.city).ToList();
        else if (sort == UserSearchByEnum.status)
            list = list.Where(p => p.userFullName.Contains(search ?? "")).OrderBy(u => u.status).ToList();
        else
            list = list.Where(p => p.userFullName.Contains(search ?? "")).OrderBy(u => u.userFullName).ToList();
        list = list.GetRange((page - 1)*pagSize, pagSize<list.Count?pagSize:list.Count);
        if (isDes)
            return list.OrderDescending().ToList();
        else
            return list;
    }

    public async Task<List<AlbumDTO>> GetAlbum(QueryAlbum queryObject)
    {
        var page = queryObject.PageNumber;
        var pageSize = queryObject.PageSize;
        var isDesc = queryObject.IsDescending;
        var sortBy = queryObject.SortBy;
        var search = queryObject.SearchTerm;

        var list = await albumRepository.getAllAlbumsAsync();

        switch (sortBy)
        {
            case AlbumSearchByEnum.AlbumTitle:
                list = list.Where(a => a.albumTitle.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.albumTitle).ToList() : list.OrderBy(a => a.albumTitle).ToList();
                break;
            case AlbumSearchByEnum.AlbumDescription:
                list = list.Where(a => a.albumDescription.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.albumDescription).ToList() : list.OrderBy(a => a.albumDescription).ToList();
                break;
            case AlbumSearchByEnum.ReleaseDate:
                list = list.Where(a => a.releaseDate.ToString("yyyy-MM-dd").Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.releaseDate).ToList() : list.OrderBy(a => a.releaseDate).ToList();
                break;
            default:
                list = list.Where(a => a.albumTitle.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
                list = isDesc ? list.OrderByDescending(a => a.albumTitle).ToList() : list.OrderBy(a => a.albumTitle).ToList();
                break;
        }

        var paginatedList = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var albumDtOs = paginatedList.Select(a => new AlbumDTO
        {
            Id = a.Id,
            albumTitle = a.albumTitle,
            albumDescription = a.albumDescription,
            releaseDate = a.releaseDate,
            imageUrl = a.AlbumPicture
        }).ToList();

        return albumDtOs;
    }
    public async Task<bool> DeleteArtist(Guid id)
    {
        return await artistRepository.DeleteArtist(id);
    }
    public async Task<bool> DeletePlaylist(Guid id)
    {
        return await playlistRepository.DeletePlaylistAsync(id)!=null;
    }

    public async Task<bool> DeleteMusic(Guid id)
    {
        return await musicRepository.DeleteMusicAsync(id);
    }

    public Task DeleteAlbum(Guid albumId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(Guid userId)
    {
        throw new NotImplementedException();
    }
}