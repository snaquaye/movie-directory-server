using Microsoft.EntityFrameworkCore;

using MovieDirectory.Infrastructure.Data;

namespace MovieDirectory.Endpoints.SearchQueryEndpoint
{
    public class SearchQuery
    {
        public string Query { get; set; }
    }

    public class SearchQueryResponse
    {
        public IEnumerable<SearchQuery> Data { get; set; }
    }

    public class GetLastFiveSearch : EndpointWithoutRequest<SearchQueryResponse>
    {
        private readonly AppDbContext _context;

        public GetLastFiveSearch(AppDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/search-queries");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var queries = await _context.SearchHistories
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => new SearchQuery
                {
                    Query = x.Query,
                }).Distinct()
                .ToListAsync();

            await SendAsync(new SearchQueryResponse
            {
                Data = queries
            });
        }
    }
}
