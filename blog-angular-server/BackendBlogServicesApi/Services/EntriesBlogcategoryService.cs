using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Entries;
using BackendBlogServicesApi.Repositories;
using BackendBlogServicesApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackendBlogServicesApi.Services
{
    public class EntriesBlogCategoryService
    {
        private readonly IEntriesBlogCategoryRepository _repository;
        private readonly EntriesBlogService _serviceBlog;

        public EntriesBlogCategoryService(IEntriesBlogCategoryRepository repository, EntriesBlogService serviceBlog)
        {
            _repository = repository;
            _serviceBlog = serviceBlog;
        }

        public async Task<Result<EntriesBlogCategoryDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null || entity.Category == null || entity.EntriesBlog == null)
            {
                return Result<EntriesBlogCategoryDto>.Failure(new EntriesBlogCategoryDto(), "Entry not found.");
            }

            var dto = new EntriesBlogCategoryDto
            {
                Id = entity.Id,
                IdEntriesBlog = entity.IdEntriesBlog,
                IdCategories = entity.IdCategories,
                CategoriaName = entity.Category.Name,
                Title = entity.EntriesBlog.Title,
                Content = entity.EntriesBlog.Content,
                Author = entity.EntriesBlog.Author,
                PublicationDate = entity.EntriesBlog.PublicationDate,
                Estado = entity.Estado
            };

            return Result<EntriesBlogCategoryDto>.Success(dto);
        }

        public async Task<Result<IEnumerable<EntriesBlogCategoryDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            if (entities == null || !entities.Any())
            {
                return Result<IEnumerable<EntriesBlogCategoryDto>>.Success(new List<EntriesBlogCategoryDto>(), "No se Encontraron");
            }

            var dtos = entities.Select(entity => new EntriesBlogCategoryDto
            {
                Id = entity.Id,
                IdEntriesBlog = entity.IdEntriesBlog,
                IdCategories = entity.IdCategories,
                CategoriaName = entity.Category.Name,
                Title = entity.EntriesBlog.Title,    
                Content = entity.EntriesBlog.Content,
                Author = entity.EntriesBlog.Author,
                PublicationDate = entity.EntriesBlog.PublicationDate,
                Estado = entity.Estado
            }).ToList();

            return Result<IEnumerable<EntriesBlogCategoryDto>>.Success(dtos);
        }

        public async Task<Result<List<EntriesBlogCategoryDto>>> AddAsync(CreateEntriesBlogCategoryDto dto)
        {
            var entityBlog = new EntriesBlogDto
            {
                Title = dto.Title,
                Content = dto.Content,
                Author = dto.Author,
                PublicationDate = dto.PublicationDate,
            };

            var entityDTO = await _serviceBlog.AddAsync(entityBlog);
            if (!entityDTO.IsSuccess)
            {
                return Result<List<EntriesBlogCategoryDto>>.Failure(new List<EntriesBlogCategoryDto>(), "Ya existe el titulo de esa Entrada");
            }

            var entriesToAdd = dto.IdCategories.Select(categoryId => new EntriesBlogCategory
            {
                IdEntriesBlog = entityDTO.Value.Id,
                IdCategories = categoryId,
                Estado = true
            }).ToList();

            var addedEntries = await _repository.AddRangeAsync(entriesToAdd);

            var categoryNames = await _repository.GetCategoryNamesByIdsAsync(dto.IdCategories);

            var resultDtos = addedEntries.Select(entry => new EntriesBlogCategoryDto
            {
                Id = entry.Id,
                IdEntriesBlog = entityDTO.Value.Id,
                IdCategories = entry.IdCategories,
                CategoriaName = categoryNames.ContainsKey(entry.IdCategories) ? categoryNames[entry.IdCategories] : string.Empty,
                Title = entityBlog.Title,
                Content = entityBlog.Content,
                Author = entityBlog.Author,
                PublicationDate = entityBlog.PublicationDate
            }).ToList();

            return Result<List<EntriesBlogCategoryDto>>.Success(resultDtos);

        }
        public async Task<Result<bool>> UpdateAsync(int id, CreateEntriesBlogCategoryDto dto)
        {
            var existingEntryBlog = await _repository.GetEntriesBlogCategoriesByIdAsync(id);
            if (existingEntryBlog == null)
            {
                return Result<bool>.Failure(false, "Entity not found.");
            }

            var EntriesBlog = new EntriesBlogDto
            {

                Title = dto.Title,
                Content = dto.Content,
                Author = dto.Author,
                PublicationDate = dto.PublicationDate,
                UpdatedAt = DateTime.Now
            };

            _serviceBlog.UpdateAsync(id, EntriesBlog);

            var existingCategoryIds = existingEntryBlog
                .Select(ec => ec.IdCategories)
                .ToHashSet();

            var categoriesToAdd = dto.IdCategories.Except(existingCategoryIds).ToList();
            var categoriesToRemove = existingCategoryIds.Except(dto.IdCategories).ToList();

            if (categoriesToRemove.Any())
            {
                await _repository.RemoveCategoriesAsync(id, categoriesToRemove);
            }

            var entriesToAdd = categoriesToAdd.Select(categoryId => new EntriesBlogCategory
            {
                IdEntriesBlog = dto.IdEntriesBlog,
                IdCategories = categoryId,
                Estado = true
            }).ToList();

            if (entriesToAdd.Any())
            {
                await _repository.AddRangeAsync(entriesToAdd);
            }

            return Result<bool>.Success(true, "Actualizado Correctamente.");
        }


        public async Task<Result<bool>> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return Result<bool>.Success(true, "Se ha Eliminado la Relacion Categorian Entrada blogs");
        }

        public async Task<Result<Dictionary<string, List<int>>>> GetActiveAuthorsWithUniqueBlogEntriesAsync()
        {
            var result = await _repository.GetActiveAuthorsWithUniqueBlogEntriesAsync();
            return Result<Dictionary<string, List<int>>>.Success(result, "Lista de Catalgo Authores");
        }

    }
}
