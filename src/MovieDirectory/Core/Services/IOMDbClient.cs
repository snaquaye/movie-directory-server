using MovieDirectory.Application.Model;
using MovieDirectory.Core.Exceptions;

using OneOf;

namespace MovieDirectory.Core.Services
{
    public interface IOMDbClient
    {
        Task<OneOf<Movie, MovieNotFoundExecption>> GetMoveById(string i);
        Task<IEnumerable<Movie>> MoviesSearch(string s);
    }
}