using BackendBlogServicesApi.Entries;

namespace BackendBlogServicesApi.Entity
{
    public class EntriesBlogCategory
    {
        public int Id { get; set; }
        public int IdEntriesBlog { get; set; }
        public EntriesBlog EntriesBlog { get; set; } // Relación con EntriesBlog
        public int IdCategories { get; set; }
        public Categories Category { get; set; } // Relación con Category
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool Estado { get; set; } = true;
    }
}
