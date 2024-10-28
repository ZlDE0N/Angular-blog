using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Entries;
using Microsoft.EntityFrameworkCore;

namespace BackendBlogServicesApi.Repositories
{
    public class EntriesBlogCategoryRepository : IEntriesBlogCategoryRepository
    {
        private readonly AppDbContext _context;

        public EntriesBlogCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EntriesBlogCategory> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<EntriesBlogCategory>()
                    .Include(e => e.Category)
                    .Include(e => e.EntriesBlog)
                    .Where(c => c.Estado == true)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }


        public async Task<IEnumerable<EntriesBlogCategory>> GetAllAsync()
        {

            try
            {
                return await _context.Set<EntriesBlogCategory>()
                       .Include(e => e.Category)
                       .Include(e => e.EntriesBlog)
                       .Where(c => c.Estado == true)
                       .ToListAsync(); // Devuelve la lista de entidades.
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }

        }

        public async Task<IEnumerable<EntriesBlogCategory>> AddRangeAsync(IEnumerable<EntriesBlogCategory> entities)
        {
            try
            {
                await _context.Set<EntriesBlogCategory>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                return entities;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task AddAsync(EntriesBlogCategory entriesBlogCategory)
        {
            try
            {
                await _context.Set<EntriesBlogCategory>().AddAsync(entriesBlogCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task UpdateAsync(EntriesBlogCategory entriesBlogCategory)
        {
            try
            {
                _context.Set<EntriesBlogCategory>().Update(entriesBlogCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var existingEntriesBlogCategories = await GetEntriesBlogCategoriesByIdAsync(id);
                if (existingEntriesBlogCategories == null || !existingEntriesBlogCategories.Any())
                {
                    return;
                }

                foreach (var entity in existingEntriesBlogCategories)
                {
                    entity.UpdatedAt = DateTime.Now;
                    entity.Estado = false;
                }

                await _context.SaveChangesAsync();

                return;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task<Dictionary<int, string>> GetCategoryNamesByIdsAsync(List<int> categoryIds)
        {
            try
            {
                return await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Name);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task RemoveCategoriesAsync(int blogEntryId, List<int> categoryIds)
        {
            try
            {
                var categoriesToRemove = await _context.Set<EntriesBlogCategory>()
                .Where(e => e.IdEntriesBlog == blogEntryId && categoryIds.Contains(e.IdCategories))
                .ToListAsync();

                _context.Set<EntriesBlogCategory>().RemoveRange(categoriesToRemove);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }

        }

        public async Task<List<int>> GetCategoriesByBlogIdAsync(int blogEntryId)
        {
            try
            {
                return await _context.Set<EntriesBlogCategory>()
                .Where(e => e.IdEntriesBlog == blogEntryId)
                .Select(e => e.IdCategories)
                .ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task<List<EntriesBlogCategory>> GetEntriesBlogCategoriesByIdAsync(int id)
        {
            try
            {
                return await _context.Set<EntriesBlogCategory>()
                    .Include(c => c.Category)
                    .Include(e => e.EntriesBlog)
               .Where(ebc => ebc.IdEntriesBlog == id && ebc.Estado == true)
               .ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<EntriesBlogCategory> categories)
        {
            try
            {
                _context.Set<EntriesBlogCategory>().UpdateRange(categories);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task<Dictionary<string, List<int>>> GetActiveAuthorsWithUniqueBlogEntriesAsync()
        {
            try
            {

                var authorsWithEntries = await _context.EntriesBlogCategory
                    .Include(e => e.EntriesBlog)
                    .Where(c => c.Estado == true && c.EntriesBlog.Estado == true)
                    .GroupBy(c => c.EntriesBlog.Author)
                    .ToDictionaryAsync(
                        g => g.Key,
                        g => g
                            .Select(c => c.EntriesBlog.Id)
                            .Distinct()
                            .ToList()
                    );

                foreach (var author in authorsWithEntries.Keys.ToList())
                {
                    var activeBlogIds = authorsWithEntries[author].Where(id =>
                        _context.EntriesBlog.Any(b => b.Id == id && b.Estado == true)).ToList();

                    if (activeBlogIds.Any())
                    {
                        authorsWithEntries[author] = activeBlogIds;
                    }
                    else
                    {
                        authorsWithEntries.Remove(author);
                    }
                }

                return authorsWithEntries;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while accessing the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

    }
}
