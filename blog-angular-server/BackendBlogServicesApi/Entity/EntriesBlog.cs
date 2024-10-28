namespace BackendBlogServicesApi.Entity
{
    public class EntriesBlog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ?UpdatedAt { get; set; } = null;
        public bool Estado { get; set; } = true;

        //public ICollection<EntriesBlogCategory> EntriesBlogCategories { get; set; }
    }
}
