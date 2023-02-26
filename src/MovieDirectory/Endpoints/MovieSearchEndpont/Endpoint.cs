using MovieDirectory.Application.Model;

namespace MovieDirectory.Endpoints.MovieSearchEndpont
{
    public class MovieSearchResult
    {
        public string Title { get; set; }
        public string Plot { get; set; }
        public string Actors { get; set; }
        public Uri Poster { get; set; }
    }

    public class MovieSearchResponse
    { 
        public IEnumerable<Movie> Data { get; set; }
    }

    public class MovieSearchEndpoint : EndpointWithoutRequest<MovieSearchResponse>
    {
    }
}
