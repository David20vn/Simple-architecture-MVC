using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Inventory? GetById(int id);
        Inventory? GetByProductId(int productId);
        IEnumerable<Inventory> GetAll();
        void Add(Inventory inventory);
    }
}