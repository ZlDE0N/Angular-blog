using BackendBlogServicesApi.Entity;
using System;

namespace BackendBlogServicesApi.Entries
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ? UpdatedAt { get; set; } = null;
        public bool Estado { get; set; } = true;

        //public ICollection<EntriesBlogCategory> EntriesBlogCategories { get; set; }
    }
}
