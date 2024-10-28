using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendBlogServicesApi.Controllers
{
    [ApiController]
    [Route("api/entity")]
    public class EntriesBlogCategoryController : ControllerBase
    {
        private readonly EntriesBlogCategoryService _service;

        public EntriesBlogCategoryController(EntriesBlogCategoryService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateEntriesBlogCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.AddAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CreateEntriesBlogCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateAsync(id, dto);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result);
        }


        [HttpGet("GetAuthorCatalogByCategory")]
        public async Task<IActionResult> GetActiveAuthorsWithUniqueBlogEntriesAsync()
        {
            var result = await _service.GetActiveAuthorsWithUniqueBlogEntriesAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result);
        }
    }
}
