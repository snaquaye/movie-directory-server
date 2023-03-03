using System.Security.Cryptography.X509Certificates;

using MovieDirectory.Application.Model;
using MovieDirectory.Core.Services;

namespace MovieDirectory.Endpoints.MovieEndponts
{
    public class GetMovieRequest
    {
        public string? Id { get; set; }
    }

    public class GetMovie : Endpoint<GetMovieRequest, Movie>
    {
        private readonly IOMDbClient _client;

        public GetMovie(IOMDbClient client)
        {
            _client = client;
        }

        public override void Configure()
        {
            Get("/movies/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetMovieRequest request, CancellationToken ct)
        {
            if (request.Id == null)
            {
                ThrowError("No movie Id was specified");
                return;
            }

            var result = await _client.GetMovieAsync(request.Id);

            if (result.IsSuccess)
            {
                await SendAsync(result.Value);

                return;
            }

            await SendAsync(result);
        }
    }
}
