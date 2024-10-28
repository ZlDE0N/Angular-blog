using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendBlogServicesApi.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Error);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var result = await _categoryService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            try
            {
                var result = await _categoryService.AddAsync(categoryDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
        {
            try
            {
                var result = await _categoryService.UpdateAsync(id,categoryDto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteAsync(id);
                if (!result.IsSuccess)
                {
                    return NotFound(result.Error);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud: " + ex.Message);
            }
        }
    }
}
