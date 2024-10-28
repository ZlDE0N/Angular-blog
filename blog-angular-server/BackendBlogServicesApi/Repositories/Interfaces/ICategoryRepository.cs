using System.Collections.Generic;
using System.Threading.Tasks;
using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.Entries;

namespace BackendBlogServicesApi.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Categories> GetByIdAsync(int id);
        Task<IEnumerable<Categories>> GetAllAsync();
        Task AddAsync(Categories category);
        Task UpdateAsync(Categories category);
        Task DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    }
}