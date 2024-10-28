using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entries;
using BackendBlogServicesApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendBlogServicesApi.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Tag = category.Tag,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                Estado = category.Estado
            };

            if (category == null)
            {
                return Result<CategoryDto>.Failure(categoryDto, $"Categoría con ID '{id}' no encontrada.");
            }

            return Result<CategoryDto>.Success(categoryDto);
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = new List<CategoryDto>();

            foreach (var category in categories)
            {
                categoryDtos.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Tag = category.Tag,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    Estado = category.Estado
                });
            }

            return Result<IEnumerable<CategoryDto>>.Success(categoryDtos);
        }

        public async Task<Result<bool>> AddAsync(CategoryDto categoryDto)
        {
            var existingNameCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.Name);
            if (existingNameCategory)
            {
                return Result<bool>.Failure(false, $"El Nombre '{categoryDto.Name}' ya existe.");
            }

            var category = new Categories
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Tag = categoryDto.Tag,
                CreatedAt = categoryDto.CreatedAt,
                UpdatedAt = categoryDto.UpdatedAt,
                Estado = categoryDto.Estado
            };

            try
            {
                await _categoryRepository.AddAsync(category);
                return Result<bool>.Success(true, "Categoría añadida exitosamente.");
            }

            catch (DbUpdateException ex)
            {
                return Result<bool>.Failure(false, $"Error al agregar la categoría: {ex.Message} " + $"{ex.InnerException}");
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción no prevista
                return Result<bool>.Failure(false, $"Error inesperado: {ex.Message} " + $"{ex.InnerException}");
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, CategoryDto categoryDto)
        {

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return Result<bool>.Failure(false, "Categoría no encontrada.");
            }

            if (existingCategory.Name != categoryDto.Name)
            {
                var existingNameCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.Name);
                if (existingNameCategory)
                {
                    return Result<bool>.Failure(false, $"El nombre '{categoryDto.Name}' ya existe, está ocupado por otra categoría.");
                }
            }

            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description;
            existingCategory.Tag = categoryDto.Tag;
            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.Estado = categoryDto.Estado;

            try
            {
                await _categoryRepository.UpdateAsync(existingCategory);
                return Result<bool>.Success(true, "Categoría actualizada exitosamente.");
            }

            catch (DbUpdateException ex)
            {
                return Result<bool>.Failure(false, $"Error al agregar la categoría: {ex.Message} " + $"{ex.InnerException}");
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción no prevista
                return Result<bool>.Failure(false, $"Error inesperado: {ex.Message} " + $"{ex.InnerException}");
            }
        }


        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                await _categoryRepository.DeleteAsync(id);
                return Result<bool>.Success(true, "Categoría eliminada exitosamente.");
            }

            catch (DbUpdateException ex)
            {
                return Result<bool>.Failure(false, $"Error al agregar la categoría: {ex.Message} " + $"{ex.InnerException}");
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción no prevista
                return Result<bool>.Failure(false, $"Error inesperado: {ex.Message} " + $"{ex.InnerException}");
            }
        }
    }
}
