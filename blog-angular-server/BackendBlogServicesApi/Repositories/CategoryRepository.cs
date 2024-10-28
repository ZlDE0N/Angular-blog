using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using BackendBlogServicesApi.Entries;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BackendBlogServicesApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Categories> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task AddAsync(Categories category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Categories category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);

            if (category != null)
            {
                category.Estado = false;
                category.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(category);
            }
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && (!excludeId.HasValue || c.Id != excludeId.Value));
        }
    }
}
