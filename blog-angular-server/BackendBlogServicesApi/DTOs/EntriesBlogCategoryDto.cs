using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Entries;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendBlogServicesApi.DTOs
{
    public class CreateEntriesBlogCategoryDto
    {
        public int ?Id { get; set; }
        public int IdEntriesBlog { get; set; }
        public List<int> IdCategories { get; set; } // Corrected type to List<int>
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool Estado { get; set; } = true;
    }


    public class EntriesBlogCategoryDto
    {
        public int? Id { get; set; }
        public int IdEntriesBlog { get; set; }
        public int IdCategories { get; set; }
        public string CategoriaName { get; set; } // Nombre de la categoría
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool Estado { get; set; }
    }
}
