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

        public Inventory AddStock(int productId, int quantity)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID del producto debe ser positivo.");
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a agregar debe ser mayor a cero.");

            var product = _productRepository.GetById(productId)
                ?? throw new KeyNotFoundException($"Producto con ID {productId} no encontrado.");

            var inventory = _inventoryRepository.GetByProductId(productId);
            if (inventory == null)
            {
                inventory = new Inventory(productId, 0);
                _inventoryRepository.Add(inventory);
            }

            inventory.AddStock(quantity);

            // Registrar movimiento de entrada
            var movement = new InventoryMovement(
                productId: productId,
                quantity: quantity,
                type: MovementType.Entry,
                description: "Reabastecimiento de inventario"
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

            inventory.SubtractStock(quantity);

            // Crear descripción adecuada según si es venta o salida manual
            string description = saleId.HasValue
                ? $"Venta #{saleId.Value}"
                : "Salida manual de inventario";

            var movement = new InventoryMovement(
                productId: productId,
                quantity: quantity,
                type: MovementType.Exit,
                description: description,
                relatedSaleId: saleId
            );
            _movementRepository.Add(movement);

            return inventory;
        }
    }
}