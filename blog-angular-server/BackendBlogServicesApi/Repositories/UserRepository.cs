
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Repositories.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace BackendBlogServicesApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }


        public async Task<bool> DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Estado = false;
                user.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null)
        {
            var normalizedUsername = username.ToLower();
            var result = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == normalizedUsername && (!excludeId.HasValue || u.Id != excludeId.Value));
            return await Task.FromResult(result);
        }


        public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
        {
            var normalizedEmail = email.ToLower();
            var result = await _context.Users
                .AnyAsync(u => u.email.ToLower() == normalizedEmail && (!excludeId.HasValue || u.Id != excludeId.Value));
            return await Task.FromResult(result);
        }

        public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == usernameOrEmail.ToLower()
                                       || u.email.ToLower() == usernameOrEmail.ToLower());
        }

        private static DbSet<T> FakeDbSet<T>(List<T> data) where T : class
        {
            var _data = data.AsQueryable();
            var fakeDbSet = Substitute.For<DbSet<T>, IQueryable<T>>();
            ((IQueryable<T>)fakeDbSet).Provider.Returns(_data.Provider);
            ((IQueryable<T>)fakeDbSet).Expression.Returns(_data.Expression);
            ((IQueryable<T>)fakeDbSet).ElementType.Returns(_data.ElementType);
            ((IQueryable<T>)fakeDbSet).GetEnumerator().Returns(_data.GetEnumerator());

            fakeDbSet.AsQueryable().Returns(fakeDbSet);

            return fakeDbSet;
        }

    }
}