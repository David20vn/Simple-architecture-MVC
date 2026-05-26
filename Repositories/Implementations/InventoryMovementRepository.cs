using Sistema_inventario_mvc.Data;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;

namespace Sistema_inventario_mvc.Repositories.Implementations
{
    public class InventoryMovementRepository : IInventoryMovementRepository
    {
        private List<InventoryMovement> _movements => InMemoryData.InventoryMovements;

        public void Add(InventoryMovement movement)
        {
            int newId = _movements.Count > 0 ? _movements.Max(m => m.Id) + 1 : 1;
            movement.SetId(newId);
            _movements.Add(movement);
        }

        public IEnumerable<InventoryMovement> GetAll()
        {
            return _movements;
        }

        public IEnumerable<InventoryMovement> GetByProductId(int productId)
        {
            return _movements.Where(m => m.ProductId == productId);
        }

        public IEnumerable<InventoryMovement> GetByDateRange(DateTime from, DateTime to)
        {
            return _movements.Where(m => m.Date >= from && m.Date <= to);
        }
    }
}