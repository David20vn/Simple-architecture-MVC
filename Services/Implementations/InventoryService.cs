using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public InventoryService(IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public IEnumerable<Inventory> GetAll()
        {
            return _inventoryRepository.GetAll();
        }

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

            // Validar que el producto existe
            var product = _productRepository.GetById(productId);
            if (product == null)
                throw new KeyNotFoundException($"Producto con ID {productId} no encontrado.");

            // Obtener inventario existente
            var inventory = _inventoryRepository.GetByProductId(productId);
            if (inventory == null)
            {
                inventory = new Inventory(productId, 0); // stock inicial 0
                _inventoryRepository.Add(inventory);      // el repositorio asigna ID
            }

            // Usar el método encapsulado para aumentar stock
            inventory.AddStock(quantity);
            // No es necesario hacer nada más, porque el repositorio trabaja con la misma referencia.
            // Si el repositorio requiere un Update explícito, lo llamamos aquí.
            // Asumimos que al modificar el objeto en memoria ya está actualizado.
            return inventory;
        }

        public Inventory SubtractStock(int productId, int quantity)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID del producto debe ser positivo.");
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a restar debe ser mayor a cero.");

            var inventory = _inventoryRepository.GetByProductId(productId);
            if (inventory == null)
                throw new InvalidOperationException($"No hay inventario para el producto con ID {productId}.");

            // El propio método SubtractStock ya lanza InvalidOperationException si no hay suficiente stock
            inventory.SubtractStock(quantity);
            return inventory;
        }
    }
}