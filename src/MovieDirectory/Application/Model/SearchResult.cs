namespace MovieDirectory.Application.Model
{
    public enum TypeEnum
    {
        Movie,
        Series,
    }

    public class SearchResult
    {
        public IList<Movie>? Search { get; set; }
        public string? TotalResults { get; set; }
        public string? Response { get; set; }
    }
}
