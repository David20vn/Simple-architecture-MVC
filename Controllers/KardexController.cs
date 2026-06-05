using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Solo usuarios autenticados (Admin o Employee)
    public class KardexController : ControllerBase
    {
        private readonly IKardexService _kardexService;

        public KardexController(IKardexService kardexService)
        {
            _kardexService = kardexService;
        }

        // GET: api/kardex/product/{productId}?from=...&to=...
        [HttpGet("product/{productId}")]
        public IActionResult GetKardex(int productId, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            // Validación básica de rango de fechas
            if (from.HasValue && to.HasValue && from > to)
                return BadRequest(new { message = "'from' no puede ser mayor que 'to'." });

            try
            {
                var kardex = _kardexService.GetKardex(productId, from, to);
                return Ok(kardex);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}