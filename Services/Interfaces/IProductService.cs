using InventorySalesAPI.Models;

namespace InventorySalesAPI.Services.Interfaces
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