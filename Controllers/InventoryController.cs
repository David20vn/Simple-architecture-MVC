using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Todo el controlador requiere autenticación
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // 1. Consultar inventario (todos los registros) – Admin y Empleado
        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _inventoryService.GetAll();
            var dtos = inventories.Select(MapToResponseDto);
            return Ok(dtos);
        }

        // 2. Consultar inventario por ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var inventory = _inventoryService.GetById(id);
                if (inventory == null)
                    return NotFound(new { message = $"Inventario con ID {id} no encontrado." });
                return Ok(MapToResponseDto(inventory));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 3. Consultar inventario por producto (movimiento específico)
        [HttpGet("product/{productId}")]
        public IActionResult GetByProductId(int productId)
        {
            var inventory = _inventoryService.GetByProductId(productId);
            if (inventory == null)
                return NotFound(new { message = $"No hay registro de inventario para el producto con ID {productId}." });
            return Ok(MapToResponseDto(inventory));
        }

        // 4. Reabastecer inventario (entrada de stock) – solo Admin
        [HttpPost("replenish")]
        [Authorize(Roles = "Admin")]
        public IActionResult Replenish([FromBody] InventoryReplenishDto dto)
        {
            if (dto.Quantity <= 0)
                return BadRequest(new { message = "La cantidad debe ser mayor a cero." });

            try
            {
                var updatedInventory = _inventoryService.AddStock(dto.ProductId, dto.Quantity);
                return Ok(MapToResponseDto(updatedInventory));
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

        private InventoryResponseDto MapToResponseDto(Models.Inventory inventory)
        {
            return new InventoryResponseDto
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                StockQuantity = inventory.StockQuantity
            };
        }
    }
}