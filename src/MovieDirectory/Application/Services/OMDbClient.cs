using MovieDirectory.Application.Model;
using MovieDirectory.Core.Exceptions;
using MovieDirectory.Core.Services;

using OneOf;

using RestSharp;

namespace MovieDirectory.Application.Services
{
    public class OMDbClient : IOMDbClient
    {
        private readonly Uri BaseUri = new Uri("http://www.omdbapi.com/");
        private readonly RestClient _client;

        public OMDbClient()
        {
            _client = new RestClient(BaseUri).AddDefaultQueryParameter("apiKey", "");
        }

        public async Task<IEnumerable<Movie>> MoviesSearch(string s)
        {
            var request = new RestRequest().AddQueryParameter("s", s);
            var movies = await _client.GetAsync<IEnumerable<Movie>>(request);

            return movies;
        }

        public async Task<OneOf<Movie, MovieNotFoundExecption>> GetMoveById(string i)
        {
            var request = new RestRequest().AddQueryParameter("i", i);
            var movie = await _client.GetAsync<Movie>(request);

            if (movie == null)
            {
                return new MovieNotFoundExecption("Movie was not found");
            }

            return movie;
        }
    }
}
