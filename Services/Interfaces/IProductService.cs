using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface IProductService
    {
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByCategory(int categoryId);
        IEnumerable<Product> GetAvailable();
        IEnumerable<Product> GetLowStock(int threshold);
        Product Create(Product product);
        Product Update(int id, Product updatedProduct);
        void UpdatePrice(int id, decimal newPrice);
        void Delete(int id);
    }
}