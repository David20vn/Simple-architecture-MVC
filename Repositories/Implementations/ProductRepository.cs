using InventorySalesAPI.Data;
using InventorySalesAPI.Models;
using InventorySalesAPI.Repositories.Interfaces;

namespace InventorySalesAPI.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        public void Add(Product product)
        {
            product.Id = InMemoryData.GetNextProductId();
            InMemoryData.Products.Add(product);
        }

        public void Update(Product product)
        {
            var existingProduct = InMemoryData.Products.FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"No existe un producto con Id {product.Id}");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
        }

        public void Delete(int id)
        {
            var existingProduct = InMemoryData.Products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"No existe un producto con Id {id}");
            }

            InMemoryData.Products.Remove(existingProduct);
        }

        public Product? GetById(int id)
        {
            return InMemoryData.Products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetAll()
        {
            return InMemoryData.Products;
        }
    }
}