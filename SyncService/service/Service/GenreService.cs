using core.Dtos.Genre;
using core.Models;
using core.Objects;
using Microsoft.AspNetCore.Internal;
using Microsoft.IdentityModel.Tokens;
using repository.Repository;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Genre> CreateGenreAsync(GenreDTO genre)
    {
        Genre addGenre = new Genre
        {
            Id = Guid.NewGuid(),
            genreName = genre.genreName,
            genreDescription = genre.genreDescription
        };
        return await _genreRepository.CreateGenreAsync(addGenre);
    }

    public async Task<bool> DeleteGenreByIdAsync(Guid id)
    {
        return await _genreRepository.DeleteGenreByIdAsync(id);
    }

    public async Task<List<Genre>> GetAllGenre(QueryObject query)
    {
        var genres = await _genreRepository.GetAllGenre();

        if (!string.IsNullOrWhiteSpace(query.Name))
            genres = genres.Where(g => g.genreName.Contains(query.Name, StringComparison.OrdinalIgnoreCase)||
                                       g.genreDescription.Contains(query.Name, StringComparison.OrdinalIgnoreCase))
                           .ToList();

        if (!string.IsNullOrWhiteSpace(query.SortBy) && query.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            genres = query.IsDecsending
                ? genres.OrderByDescending(g => g.genreName).ToList()
                : genres.OrderBy(g => g.genreName).ToList();
        else
            genres = query.IsDecsending
                ? genres.OrderByDescending(g => g.genreDescription).ToList()
                : genres.OrderBy(g => g.genreDescription).ToList();


        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var paginatedAlbums = genres.Skip(skipNumber).Take(query.PageSize).ToList();

        return paginatedAlbums;
    }

    public async Task<Genre> GetGenreByIdAsync(Guid id)
    {
        return await _genreRepository.GetGenreById(id);
    }

    public async Task<Genre> UpdateGenreByIdAsync(GenreDTO genre, Guid id)
    {
        var newGenre = await _genreRepository.GetGenreById(id);
        if (!genre.genreName.IsNullOrEmpty())
        {
            newGenre.genreName = genre.genreName;
        }
        if(!genre.genreDescription.IsNullOrEmpty())
        {
            newGenre.genreDescription = genre.genreDescription;
        }

        return await _genreRepository.UpdateGenreAsync(newGenre);
    }
}