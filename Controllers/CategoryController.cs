using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // requiere autenticación para todo el controlador
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        // Listar todas las categorías (accesible por Admin y Empleado)
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryService.GetAll();
            var dtos = categories.Select(MapToResponseDto);
            return Ok(dtos);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var category = _categoryService.GetById(id);
                if (category == null)
                    return NotFound(new { message = $"Categoría con ID {id} no encontrada." });
                return Ok(MapToResponseDto(category));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CreateCategoryRequestDto dto)
        {
            try
            {
                var category = new Category(dto.Name);
                var created = _categoryService.Create(category);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponseDto(created));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/categories/{id}  (solo Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                _categoryService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private CategoryResponseDto MapToResponseDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}