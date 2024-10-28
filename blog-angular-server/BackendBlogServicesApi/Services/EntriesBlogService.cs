using BackendBlogServicesApi.Data;
using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendBlogServicesApi.Services
{
    public class EntriesBlogService
    {
        private readonly IEntriesBlogRepository _entriesBlogRepository;

        public EntriesBlogService(IEntriesBlogRepository entriesBlogRepository)
        {
            _entriesBlogRepository = entriesBlogRepository;
        }

        public async Task<Result<EntriesBlogDto>> GetByIdAsync(int id)
        {
            var entry = await _entriesBlogRepository.GetByIdAsync(id);
            if (entry == null)
            {
                return Result<EntriesBlogDto>.Failure(null, $"Entrada de blog con ID '{id}' no encontrada.");
            }

            var entryDto = new EntriesBlogDto
            {
                Id = entry.Id,
                Title = entry.Title,
                Content = entry.Content,
                Author = entry.Author,
                PublicationDate = entry.PublicationDate,
                CreatedAt = entry.CreatedAt,
                UpdatedAt = entry.UpdatedAt,
                Estado = entry.Estado
            };

            return Result<EntriesBlogDto>.Success(entryDto);
        }

        public async Task<Result<IEnumerable<EntriesBlogDto>>> GetAllAsync()
        {
            var entries = await _entriesBlogRepository.GetAllAsync();
            var entryDtos = new List<EntriesBlogDto>();

            foreach (var entry in entries)
            {
                entryDtos.Add(new EntriesBlogDto
                {
                    Id = entry.Id,
                    Title = entry.Title,
                    Content = entry.Content,
                    Author = entry.Author,
                    PublicationDate = entry.PublicationDate,
                    CreatedAt = entry.CreatedAt,
                    UpdatedAt = entry.UpdatedAt,
                    Estado = entry.Estado
                });
            }

            return Result<IEnumerable<EntriesBlogDto>>.Success(entryDtos);
        }

        public async Task<Result<EntriesBlogDto>> AddAsync(EntriesBlogDto entryDto)
        {
            if (await _entriesBlogRepository.ExistsByTitleAsync(entryDto.Title))
            {
                return Result<EntriesBlogDto>.Failure(new EntriesBlogDto(), $"El título '{entryDto.Title}' ya existe.");
            }

            var entry = new EntriesBlog
            {
                Title = entryDto.Title,
                Content = entryDto.Content,
                Author = entryDto.Author,
                PublicationDate = entryDto.PublicationDate,
                CreatedAt = entryDto.CreatedAt,
                UpdatedAt = entryDto.UpdatedAt,
                Estado = entryDto.Estado
            };

            var entity = await _entriesBlogRepository.AddAsync(entry);
            var entityDTO = new EntriesBlogDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                Author = entity.Author,
                PublicationDate = entity.PublicationDate,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Estado = entity.Estado
            };

            return Result<EntriesBlogDto>.Success(entityDTO, "Entrada de blog añadida exitosamente.");
        }

        public async Task<Result<bool>> UpdateAsync(int id, EntriesBlogDto entryDto)
        {
            var existingEntry = await _entriesBlogRepository.GetByIdAsync(id);
            if (existingEntry == null)
            {
                return Result<bool>.Failure(false, "Entrada de blog no encontrada.");
            }

            if (existingEntry.Title != entryDto.Title && await _entriesBlogRepository.ExistsByTitleAsync(entryDto.Title))
            {
                return Result<bool>.Failure(false, $"El título '{entryDto.Title}' ya existe.");
            }

            existingEntry.Title = entryDto.Title;
            existingEntry.Content = entryDto.Content;
            existingEntry.Author = entryDto.Author;
            existingEntry.PublicationDate = entryDto.PublicationDate;
            existingEntry.UpdatedAt = DateTime.UtcNow;
            existingEntry.Estado = entryDto.Estado;

            await _entriesBlogRepository.UpdateAsync(existingEntry);
            return Result<bool>.Success(true, "Entrada de blog actualizada exitosamente.");
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            await _entriesBlogRepository.DeleteAsync(id);
            return Result<bool>.Success(true, "Entrada de blog eliminada exitosamente.");
        }

        public async Task<Result<List<string>>> GetAllAuthor()
        {
            var result = await _entriesBlogRepository.GetAllAuthor();
            return Result<List<string>>.Success(result, "Catalgo Lista de Autores.");
        }
    }
}
