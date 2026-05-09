using Sistema_inventario_mvc.Data;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;

namespace Sistema_inventario_mvc.Repositories.Implementations
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories => InMemoryData.Inventories;

        public Inventory? GetById(int id)
        {
            return _inventories.FirstOrDefault(i => i.Id == id);
        }

        public Inventory? GetByProductId(int productId)
        {
            return _inventories.FirstOrDefault(i => i.ProductId == productId);
        }

        public IEnumerable<Inventory> GetAll()
        {
            return _inventories;
        }

        public void Add(Inventory inventory)
        {
            int newId = _inventories.Count > 0 ? _inventories.Max(i => i.Id) + 1 : 1;
            inventory.SetId(newId);
            _inventories.Add(inventory);
        }
    }
}