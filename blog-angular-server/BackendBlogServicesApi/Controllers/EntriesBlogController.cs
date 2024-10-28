using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendBlogServicesApi.Controllers
{
    [ApiController]
    [Route("api/blog")]
    public class EntriesBlogController : ControllerBase
    {
        private readonly EntriesBlogService _entriesBlogService;
        private readonly ILogger<EntriesBlogController> _logger;

        public EntriesBlogController(EntriesBlogService entriesBlogService, ILogger<EntriesBlogController> logger)
        {
            _entriesBlogService = entriesBlogService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntry(int id)
        {
            var result = await _entriesBlogService.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEntries()
        {
            var result = await _entriesBlogService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry(EntriesBlogDto entryDto)
        {
            var result = await _entriesBlogService.AddAsync(entryDto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetEntry), new { id = entryDto.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntry(int id, EntriesBlogDto entryDto)
        {
            var result = await _entriesBlogService.UpdateAsync(id, entryDto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var result = await _entriesBlogService.DeleteAsync(id);
            if (!result.IsSuccess) return NotFound(result.Error);
            return NoContent();
        }

        [HttpGet("GetAuthorCatalog")]
        public async Task<IActionResult> GetAllAuthor()
        {
            var result = await _entriesBlogService.GetAllAuthor();
            return Ok(result);
        }
    }
}
