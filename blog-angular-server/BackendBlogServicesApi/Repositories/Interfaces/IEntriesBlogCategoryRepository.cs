using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace BackendBlogServicesApi.Repositories
{
    public interface IEntriesBlogCategoryRepository
    {
        Task<EntriesBlogCategory> GetByIdAsync(int id);
        Task<IEnumerable<EntriesBlogCategory>> GetAllAsync();
        Task AddAsync(EntriesBlogCategory entriesBlogCategory);
        Task<IEnumerable<EntriesBlogCategory>> AddRangeAsync(IEnumerable<EntriesBlogCategory> entities);
        Task UpdateAsync(EntriesBlogCategory entriesBlogCategory);
        Task DeleteAsync(int id);
        Task<Dictionary<int, string>> GetCategoryNamesByIdsAsync(List<int> categoryIds);
        Task RemoveCategoriesAsync(int blogEntryId, List<int> categoryIds);
        Task<List<int>> GetCategoriesByBlogIdAsync(int blogEntryId);
        Task<List<EntriesBlogCategory>> GetEntriesBlogCategoriesByIdAsync(int id);
        Task UpdateRangeAsync(IEnumerable<EntriesBlogCategory> categories);

        Task<Dictionary<string, List<int>>> GetActiveAuthorsWithUniqueBlogEntriesAsync();
    }
}
