using Sistema_inventario_mvc.Data;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;

namespace Sistema_inventario_mvc.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        // Referencias directas a las listas en memoria
        private List<Product> _products => InMemoryData.Products;
        private List<Inventory> _inventories => InMemoryData.Inventories;

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            return _products.Where(p => p.CategoryId == categoryId);
        }

        public IEnumerable<Product> GetAvailable()
        {
            var availableProductIds = _inventories
                .Where(inv => inv.StockQuantity > 0)
                .Select(inv => inv.ProductId)
                .ToHashSet();

            return _products.Where(p => availableProductIds.Contains(p.Id));
        }

        public IEnumerable<Product> GetLowStock(int threshold)
        {
            var lowStockProductIds = _inventories
                .Where(inv => inv.StockQuantity <= threshold)
                .Select(inv => inv.ProductId)
                .ToHashSet();

            return _products.Where(p => lowStockProductIds.Contains(p.Id));
        }

        public void Add(Product product)
        {
            int newId = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            product.SetId(newId);                       
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.SetName(product.Name);
                existing.SetPrice(product.Price);
                existing.SetCategory(product.CategoryId);
            }
        }

        public void UpdatePrice(int productId, decimal newPrice)
        {
            var product = GetById(productId);
            if (product != null)
            {
                product.SetPrice(newPrice);             
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
                _products.Remove(product);
        }
    }
}