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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // ... otros endpoints (GetAll, GetById, GetByProductId) se mantienen igual

        // POST: api/inventory/replenish (solo Admin)
        [HttpPost("replenish")]
        [Authorize(Roles = "Admin")]
        public IActionResult Replenish([FromBody] InventoryReplenishDto dto)
        {
            if (dto.Quantity <= 0)
                return BadRequest(new { message = "La cantidad debe ser mayor a cero." });
            if (dto.UnitCost < 0)
                return BadRequest(new { message = "El costo unitario no puede ser negativo." });

            try
            {
                var updatedInventory = _inventoryService.AddStock(dto.ProductId, dto.Quantity, dto.UnitCost);
                return Ok(updatedInventory);
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
    }
}