namespace LinCms.Infrastructure.Resources.Books
{
    public class BookResource
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Author { get; set; }

        public string? Summary { get; set; }

        public string? Image { get; set; }
    }
}
