using core.Models;
using repository.Repository.Interfaces;
using service.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task<List<Genre>> GetAllGenre()
        {
            return await _genreRepository.GetAllGenre();
        }
    }
}
