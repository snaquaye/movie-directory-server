using System.Net;
using System.Net.Http.Json;

using Ardalis.Result;

using MovieDirectory.Application.Model;
using MovieDirectory.Core.Services;

namespace MovieDirectory.Application.Services
{
    public class OMDbClient : IOMDbClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OMDbClient> _logger;

        public OMDbClient(HttpClient httpClient, IConfiguration configuration, ILogger<OMDbClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Result<SearchResult>> MoviesSearch(string s, string page = "1")
        {
            try
            {
                var result = await _httpClient.GetAsync($"?apikey={_configuration["OMDbApiKey"]}&s={s}&page={page}");

                if (result.Content == null)
                {
                    return Result.Error("Response content was null");
                }

                result.EnsureSuccessStatusCode();

                var searchResult = await result.Content.ReadFromJsonAsync<SearchResult>();
                return Result.Success(searchResult);
            } catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);

                return Result.Error(ex.Message);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<Movie>> GetMovieAsync(string id)
        {
            var result = await _httpClient.GetAsync($"?apikey={_configuration["OMDbApiKey"]}&i={id}&plot=full");
            Console.WriteLine(await result.Content.ReadAsStringAsync());

            var movie = await result.Content.ReadFromJsonAsync<Movie>();
            
            if (result.IsSuccessStatusCode && movie != null)
            {
                return Result.Success(movie);
            }

            return Result.NotFound();
        }
    }
}
