using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product == null)
                    return NotFound(new { message = $"Producto con ID {id} no encontrado." });

                return Ok(MapToResponseDto(product));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();
            var dtos = products.Select(MapToResponseDto);
            return Ok(dtos);
        }

        // GET: api/products/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            try
            {
                var products = _productService.GetByCategory(categoryId);
                return Ok(products.Select(MapToResponseDto));
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

        // GET: api/products/available
        [HttpGet("available")]
        public IActionResult GetAvailable()
        {
            var products = _productService.GetAvailable();
            return Ok(products.Select(MapToResponseDto));
        }

        // GET: api/products/low-stock?threshold=5
        [HttpGet("low-stock")]
        public IActionResult GetLowStock([FromQuery] int threshold = 5)
        {
            try
            {
                var products = _productService.GetLowStock(threshold);
                return Ok(products.Select(MapToResponseDto));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/products  (solo Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CreateProductRequestDto dto)
        {
            try
            {
                var product = new Product(
                    name: dto.Name,
                    price: dto.Price,
                    categoryId: dto.CategoryId
                );
                var created = _productService.Create(product);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponseDto(created));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/products/{id}  (solo Admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, [FromBody] UpdateProductRequestDto dto)
        {
            try
            {
                var updatedProduct = new Product(
                    name: dto.Name,
                    price: dto.Price,
                    categoryId: dto.CategoryId
                );
                var result = _productService.Update(id, updatedProduct);
                return Ok(MapToResponseDto(result));
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

        // PATCH: api/products/{id}/price  (solo Admin)
        [HttpPatch("{id}/price")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePrice(int id, [FromBody] PriceUpdateRequestDto dto)
        {
            try
            {
                _productService.UpdatePrice(id, dto.Price);
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

        // DELETE: api/products/{id}  (solo Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                _productService.Delete(id);
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

        // Método privado de mapeo a DTO de respuesta
        private ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
        }
    }
}