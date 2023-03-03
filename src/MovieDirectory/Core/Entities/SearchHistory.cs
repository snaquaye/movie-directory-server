namespace MovieDirectory.Core.Entities
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string Query { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
