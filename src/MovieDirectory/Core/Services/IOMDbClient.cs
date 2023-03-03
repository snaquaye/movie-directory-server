using Ardalis.Result;

using MovieDirectory.Application.Model;

namespace MovieDirectory.Core.Services
{
    public interface IOMDbClient
    {
        public Task<Result<Movie>> GetMovieAsync(string id);
        Task<Result<SearchResult>> MoviesSearch(string? s, string? page);
    }
}