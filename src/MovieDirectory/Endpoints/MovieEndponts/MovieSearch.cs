using FluentValidation.Results;

using Microsoft.EntityFrameworkCore;

using MovieDirectory.Application.Model;
using MovieDirectory.Core.Entities;
using MovieDirectory.Core.Services;
using MovieDirectory.Infrastructure.Data;

namespace MovieDirectory.Endpoints.MovieSearchEndpont
{
    public class MovieSearchRequest
    {
        public string? SearchTerm { get; set; }
        public string? Page { get; set; } = "1";
    }

    public class MovieSearchResult
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string ImdbId { get; set; }
        public string Year { get; set; }
        public Uri Poster { get; set; }
    }

    public class MovieSearchResponse
    {
        public IEnumerable<MovieSearchResult>? Data { get; set; }
        public string? TotalRecords { get; set; }
    }

    public class MovieSearchPreProcessor : IPreProcessor<MovieSearchRequest>
    {
        public async Task PreProcessAsync(MovieSearchRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
        {
            var context = ctx.Resolve<AppDbContext>();

            var searchHistory = new SearchHistory
            {
                Query = req.SearchTerm,
            };

            context.SearchHistories.Add(searchHistory);
            await context.SaveChangesAsync();

            await Task.CompletedTask;
        }
    }

    public class MovieSearchEndpoint : Endpoint<MovieSearchRequest, MovieSearchResponse>
    {
        private readonly IOMDbClient _client;

        public MovieSearchEndpoint(IOMDbClient client)
        {
            _client = client;
        }

        public override void Configure()
        {
            Get("/movies");
            PreProcessors(new MovieSearchPreProcessor());
            AllowAnonymous();
        }

        public override async Task HandleAsync(MovieSearchRequest request, CancellationToken ct)
        {
            int.TryParse(request.Page, out int pageNum);

            if (pageNum > 100)
            {
                ThrowError("Page cannot be more than 100");
            }

            var searchResult = await _client.MoviesSearch(request.SearchTerm, request.Page);

            if (searchResult.IsSuccess)
            {
                var response = searchResult.Value;

                if (response.Response == "False")
                {
                    await SendAsync(new MovieSearchResponse
                    {
                        Data = new List<MovieSearchResult>(),
                        TotalRecords = response.TotalResults
                    });

                    return;
                }

                var movies = response.Search.Select((movie) => new MovieSearchResult
                {
                    Title = movie.Title,
                    Type = movie.Type,
                    Year = movie.Year,
                    Poster = movie.Poster,
                    ImdbId = movie.ImdbId,
                }).ToList();

                await SendAsync(new MovieSearchResponse
                {
                    Data = movies,
                    TotalRecords = response.TotalResults
                });

                return;
            }

            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
        }
    }
}
