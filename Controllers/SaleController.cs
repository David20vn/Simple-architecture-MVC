using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Todo requiere autenticación
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        // 1. Registrar venta (Empleado o Admin)
        [HttpPost]
        public IActionResult Create([FromBody] CreateSaleRequestDto dto)
        {
            if (dto?.Details == null || dto.Details.Count == 0)
                return BadRequest(new { message = "Debe incluir al menos un producto en la venta." });

            // Obtener ID del usuario desde el token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Token inválido, no se encontró el ID de usuario." });

            int userId = int.Parse(userIdClaim);

            // Convertir DTOs a detalles del modelo
            var details = dto.Details.Select(item =>
                new SaleDetail(productId: item.ProductId, quantity: item.Quantity, unitPrice: 0) // unitPrice se calculará en el servicio
            ).ToList();

            try
            {
                var sale = _saleService.CreateSale(userId, details);
                var responseDto = MapToResponseDto(sale);
                return CreatedAtAction(nameof(GetById), new { id = sale.Id }, responseDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 5. Consultar historial de ventas (solo Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var sales = _saleService.GetAll();
            var dtos = sales.Select(MapToResponseDto);
            return Ok(dtos);
        }

        // Consultar venta por ID (solo Admin)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetById(int id)
        {
            try
            {
                var sale = _saleService.GetById(id);
                if (sale == null)
                    return NotFound(new { message = $"Venta con ID {id} no encontrada." });
                return Ok(MapToResponseDto(sale));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 6. Consultar ventas por rango de fechas (solo Admin)
        [HttpGet("range")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from == default || to == default)
                return BadRequest(new { message = "Debe proporcionar las fechas 'from' y 'to' en formato ISO 8601." });
            if (from > to)
                return BadRequest(new { message = "'from' no puede ser mayor que 'to'." });

            var sales = _saleService.GetByDateRange(from, to);
            var dtos = sales.Select(MapToResponseDto);
            return Ok(dtos);
        }

        // Mapeo de Sale a SaleResponseDto
        private SaleResponseDto MapToResponseDto(Sale sale)
        {
            return new SaleResponseDto
            {
                Id = sale.Id,
                UserId = sale.UserId,
                Total = sale.Total,
                SaleDate = sale.SaleDate,
                Details = sale.Details.Select(d => new SaleDetailResponseDto
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    SubTotal = d.SubTotal
                }).ToList()
            };
        }
    }
}