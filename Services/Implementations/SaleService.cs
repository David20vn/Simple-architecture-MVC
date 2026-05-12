using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryService _inventoryService;

        public SaleService(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IInventoryService inventoryService)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _inventoryService = inventoryService;
        }

        public Sale CreateSale(int userId, List<SaleDetail> details)
        {
            if (details == null || details.Count == 0)
                throw new ArgumentException("La venta debe contener al menos un producto.");

            // Validar productos, obtener precios y verificar stock
            var saleDetails = new List<SaleDetail>();
            foreach (var item in details)
            {
                var product = _productRepository.GetById(item.ProductId)
                    ?? throw new KeyNotFoundException($"Producto con ID {item.ProductId} no encontrado.");

                // Validar stock suficiente (lanza InvalidOperationException si no hay)
                _inventoryService.SubtractStock(item.ProductId, item.Quantity);

                // Crear detalle con precio actual del producto
                var detail = new SaleDetail(
                    productId: product.Id,
                    quantity: item.Quantity,
                    unitPrice: product.Price
                );
                saleDetails.Add(detail);
            }

            // Crear venta (Total y SaleDate se calculan automáticamente)
            var sale = new Sale(userId, saleDetails);
            _saleRepository.Add(sale); // Asigna ID y guarda

            return sale;
        }

        public Sale? GetById(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            return _saleRepository.GetById(id);
        }

        public IEnumerable<Sale> GetAll()
        {
            return _saleRepository.GetAll();
        }

        public IEnumerable<Sale> GetByDateRange(DateTime from, DateTime to)
        {
            if (from > to) throw new ArgumentException("La fecha 'from' no puede ser mayor que 'to'.");
            return _saleRepository.GetByDateRange(from, to);
        }
    }
}