using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Repositories.Interfaces
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        Product? GetById(int id);
        List<Product> GetAll();
    }
}