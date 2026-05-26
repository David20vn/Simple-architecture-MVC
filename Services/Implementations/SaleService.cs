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

            // 1. Validar productos y construir detalles con precios actuales
            var saleDetails = new List<SaleDetail>();
            foreach (var item in details)
            {
                var product = _productRepository.GetById(item.ProductId)
                    ?? throw new KeyNotFoundException($"Producto con ID {item.ProductId} no encontrado.");

                var detail = new SaleDetail(
                    productId: product.Id,
                    quantity: item.Quantity,
                    unitPrice: product.Price
                );

                // Generar ID interno del detalle
                int newId = saleDetails.Count > 0 ? saleDetails.Max(d => d.Id) + 1 : 1;
                detail.SetId(newId);

                saleDetails.Add(detail);
            }

            // 2. Crear la venta y guardarla (asigna ID a la venta)
            var sale = new Sale(userId, saleDetails);
            _saleRepository.Add(sale);   // ahora sale.Id está disponible

            // 3. Descontar stock vinculando el ID de la venta
            try
            {
                foreach (var detail in saleDetails)
                {
                    _inventoryService.SubtractStock(detail.ProductId, detail.Quantity, sale.Id);
                }
            }
            catch
            {
                // Si algo falla (stock insuficiente inesperado), revertimos la venta
                _saleRepository.Delete(sale.Id);
                throw;
            }

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