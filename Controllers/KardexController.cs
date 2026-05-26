using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Solo usuarios autenticados (Admin y Employee)
    public class KardexController : ControllerBase
    {
        private readonly IInventoryMovementRepository _movementRepository;

        public KardexController(IInventoryMovementRepository movementRepository)
        {
            _movementRepository = movementRepository;
        }

        // GET: api/kardex
        // Parámetros opcionales: productId, from, to
        [HttpGet]
        public IActionResult GetMovements(
            [FromQuery] int? productId = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            IEnumerable<InventoryMovement> movements = _movementRepository.GetAll();

            // Aplicar filtros si vienen especificados
            if (productId.HasValue)
            {
                if (productId.Value <= 0)
                    return BadRequest(new { message = "El ID del producto debe ser positivo." });

                movements = _movementRepository.GetByProductId(productId.Value);
            }

            if (from.HasValue)
                movements = movements.Where(m => m.Date >= from.Value);

            if (to.HasValue)
                movements = movements.Where(m => m.Date <= to.Value);

            var dtos = movements.Select(MapToResponseDto).OrderByDescending(m => m.Date);
            return Ok(dtos);
        }

        // Método privado de mapeo
        private InventoryMovementResponseDto MapToResponseDto(InventoryMovement movement)
        {
            return new InventoryMovementResponseDto
            {
                Id = movement.Id,
                ProductId = movement.ProductId,
                Quantity = movement.Quantity,
                Type = movement.Type == MovementType.Entry ? "Entry" : "Exit",
                Date = movement.Date,
                Description = movement.Description,
                RelatedSaleId = movement.RelatedSaleId
            };
        }
    }
}