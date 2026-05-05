using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByCategory(int categoryId);
        IEnumerable<Product> GetAvailable();       // productos con stock > 0
        IEnumerable<Product> GetLowStock(int threshold); // stock <= threshold
        void Add(Product product);
        void Update(Product product);
        void UpdatePrice(int productId, decimal newPrice);
        void Delete(int id);
    }
}