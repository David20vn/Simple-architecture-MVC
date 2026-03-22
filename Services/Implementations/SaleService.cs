using InventorySalesAPI.Models;
using InventorySalesAPI.Repositories.Interfaces;
using InventorySalesAPI.Services.Interfaces;
using InventorySalesAPI.Data;

namespace InventorySalesAPI.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public void CreateSale(Sale sale)
        {
            if (sale.Details == null || !sale.Details.Any())
                throw new Exception("La venta debe tener al menos un detalle");

            decimal total = 0;

            foreach (var detail in sale.Details)
            {
                var product = _productRepository.GetById(detail.ProductId);

                if (product == null)
                    throw new Exception($"Producto {detail.ProductId} no existe");

                var inventory = InMemoryData.Inventories.FirstOrDefault(i => i.ProductId == detail.ProductId);

                if (inventory == null)
                    throw new Exception("No hay inventario para el producto");

                if (inventory.Stock < detail.Quantity)
                    throw new Exception("Stock insuficiente");

                // calcular subtotal
                detail.UnitPrice = product.Price;
                detail.SubTotal = detail.Quantity * product.Price;

                total += detail.SubTotal;

                // descontar stock
                inventory.Stock -= detail.Quantity;
            }

            sale.Total = total;

            _saleRepository.Add(sale);
        }

        public Sale GetById(int id)
        {
            var sale = _saleRepository.GetById(id);

            if (sale == null)
                throw new Exception("Venta no encontrada");

            return sale;
        }

        public List<Sale> GetAll()
        {
            return _saleRepository.GetAll();
        }
    }
}