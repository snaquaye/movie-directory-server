namespace MovieDirectory.Core.Exceptions
{
    public class MovieNotFoundExecption : Exception
    {
        public MovieNotFoundExecption() { }

        public MovieNotFoundExecption(string? message) : base(message)
        {
        }
    }
}
