using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class KardexService : IKardexService
    {
        private readonly IInventoryMovementRepository _movementRepository;
        private readonly IProductRepository _productRepository;

        public KardexService(
            IInventoryMovementRepository movementRepository,
            IProductRepository productRepository)
        {
            _movementRepository = movementRepository;
            _productRepository = productRepository;
        }

        public ProductKardexResponseDto GetKardex(int productId, DateTime? from = null, DateTime? to = null)
        {
            var product = _productRepository.GetById(productId)
                ?? throw new KeyNotFoundException($"Producto con ID {productId} no encontrado.");

            // Obtener movimientos ordenados
            var movements = _movementRepository.GetByProductIdOrdered(productId);

            // Aplicar filtros de fecha si se especificaron
            if (from.HasValue)
                movements = movements.Where(m => m.Date >= from.Value);
            if (to.HasValue)
                movements = movements.Where(m => m.Date <= to.Value);

            // Variables acumuladoras
            int currentQuantity = 0;
            decimal currentTotalValue = 0;
            int cumulativeEntryQuantity = 0;
            var rows = new List<KardexRowDto>();

            foreach (var mov in movements)
            {
                // Actualizar acumuladores según tipo de movimiento
                if (mov.Type == MovementType.Entry)
                {
                    currentQuantity += mov.Quantity;
                    currentTotalValue += mov.TotalCost;
                    cumulativeEntryQuantity += mov.Quantity;
                }
                else // Exit
                {
                    currentQuantity -= mov.Quantity;
                    currentTotalValue -= mov.TotalCost;
                }

                // Calcular costo promedio actual
                decimal averageCost = currentQuantity > 0 ? currentTotalValue / currentQuantity : 0;

                // Construir fila del kardex
                var row = new KardexRowDto
                {
                    MovementId = mov.Id,
                    Date = mov.Date,
                    Description = mov.Description,

                    // Entradas
                    EntryQuantity = mov.Type == MovementType.Entry ? mov.Quantity : null,
                    EntryUnitCost = mov.Type == MovementType.Entry ? mov.UnitCost : null,
                    EntryTotalCost = mov.Type == MovementType.Entry ? mov.TotalCost : null,

                    // Salidas
                    ExitQuantity = mov.Type == MovementType.Exit ? mov.Quantity : null,
                    ExitUnitCost = mov.Type == MovementType.Exit ? mov.UnitCost : null,
                    ExitTotalCost = mov.Type == MovementType.Exit ? mov.TotalCost : null,

                    // Acumulado de entradas
                    CumulativeEntryQuantity = cumulativeEntryQuantity,

                    // Saldos
                    BalanceQuantity = currentQuantity,
                    BalanceAverageCost = averageCost,
                    BalanceTotalValue = currentTotalValue
                };

                rows.Add(row);
            }

            return new ProductKardexResponseDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Rows = rows
            };
        }
    }
}