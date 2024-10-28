using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendBlogServicesApi.Repositories
{
    public class EntriesBlogRepository : IEntriesBlogRepository
    {
        private readonly AppDbContext _context;

        public EntriesBlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EntriesBlog> GetByIdAsync(int id)
        {
            return await _context.EntriesBlog.FindAsync(id);
        }

        public async Task<IEnumerable<EntriesBlog>> GetAllAsync()
        {
            return await _context.EntriesBlog.ToListAsync();
        }

        public async Task<EntriesBlog> AddAsync(EntriesBlog entry)
        {
            await _context.EntriesBlog.AddAsync(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task UpdateAsync(EntriesBlog entry)
        {
            _context.EntriesBlog.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await GetByIdAsync(id);
            if (entry != null)
            {
                entry.Estado = false;
                entry.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(entry);
            }
        }

        public async Task<bool> ExistsByTitleAsync(string title, int? excludeId = null)
        {
            return await _context.EntriesBlog
                .AnyAsync(e => e.Title == title && (!excludeId.HasValue || e.Id != excludeId.Value));
        }

        public async Task<List<string>> GetAllAuthor()
        {
            return await _context.EntriesBlog
                    .Select(e => e.Author)
                    .Distinct()
                    .ToListAsync();
        }
    }
}
