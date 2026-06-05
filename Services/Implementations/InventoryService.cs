using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryMovementRepository _movementRepository;

        public InventoryService(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IInventoryMovementRepository movementRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _movementRepository = movementRepository;
        }

        public IEnumerable<Inventory> GetAll() => _inventoryRepository.GetAll();

        public Inventory? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.");
            return _inventoryRepository.GetById(id);
        }

        public Inventory? GetByProductId(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID del producto debe ser positivo.");
            return _inventoryRepository.GetByProductId(productId);
        }

        public Inventory AddStock(int productId, int quantity, decimal unitCost)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID del producto debe ser positivo.");
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a agregar debe ser mayor a cero.");
            if (unitCost < 0)
                throw new ArgumentException("El costo unitario no puede ser negativo.");

            var product = _productRepository.GetById(productId)
                ?? throw new KeyNotFoundException($"Producto con ID {productId} no encontrado.");

            var inventory = _inventoryRepository.GetByProductId(productId);
            if (inventory == null)
            {
                inventory = new Inventory(productId, 0);
                _inventoryRepository.Add(inventory);
            }

            inventory.AddStock(quantity);

            // Registrar movimiento de entrada con costo
            var movement = new InventoryMovement(
                productId: productId,
                quantity: quantity,
                unitCost: unitCost,
                type: MovementType.Entry,
                description: $"Reabastecimiento - Costo unitario: {unitCost:C}"
            );
            _movementRepository.Add(movement);

            return inventory;
        }

        public Inventory SubtractStock(int productId, int quantity, int? saleId = null)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID del producto debe ser positivo.");
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a restar debe ser mayor a cero.");

            var inventory = _inventoryRepository.GetByProductId(productId)
                ?? throw new InvalidOperationException($"No hay inventario para el producto con ID {productId}.");

            // Obtener el costo promedio actual para valorizar la salida
            var (currentQuantity, averageCost, _) = GetCurrentBalance(productId);

            if (currentQuantity < quantity)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {currentQuantity}, solicitado: {quantity}.");

            // Actualizar el inventario físico
            inventory.SubtractStock(quantity);

            // Crear descripción para el movimiento
            string description = saleId.HasValue
                ? $"Venta #{saleId.Value}"
                : "Salida de inventario";

            // Registrar movimiento de salida con el costo promedio calculado
            var movement = new InventoryMovement(
                productId: productId,
                quantity: quantity,
                unitCost: averageCost,
                type: MovementType.Exit,
                description: description,
                relatedSaleId: saleId
            );
            _movementRepository.Add(movement);

            return inventory;
        }

        // Método privado para calcular el saldo actual (cantidad, costo promedio, valor total)
        private (int quantity, decimal averageCost, decimal totalValue) GetCurrentBalance(int productId)
        {
            var movements = _movementRepository.GetByProductIdOrdered(productId);

            int quantity = 0;
            decimal totalValue = 0;

            foreach (var mov in movements)
            {
                if (mov.Type == MovementType.Entry)
                {
                    quantity += mov.Quantity;
                    totalValue += mov.TotalCost;
                }
                else // Exit
                {
                    // Para salidas, usar el costo promedio al momento de la salida (ya registrado)
                    quantity -= mov.Quantity;
                    totalValue -= mov.TotalCost; // El TotalCost de la salida fue calculado con el promedio de ese momento
                }
            }

            decimal averageCost = quantity > 0 ? totalValue / quantity : 0;
            return (quantity, averageCost, totalValue);
        }
    }
}