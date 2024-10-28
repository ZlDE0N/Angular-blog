namespace BackendBlogServicesApi.DTOs
{
    public class EntriesBlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ?UpdatedAt { get; set; }
        public bool Estado { get; set; }
    }
}
