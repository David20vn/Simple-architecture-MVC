using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface IProductService
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        Product GetById(int id);
        List<Product> GetAll();
    }
}