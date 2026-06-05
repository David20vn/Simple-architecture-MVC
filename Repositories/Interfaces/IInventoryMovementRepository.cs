using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Repositories.Interfaces
{
    public interface IInventoryMovementRepository
    {
        void Add(InventoryMovement movement);
        IEnumerable<InventoryMovement> GetAll();
        IEnumerable<InventoryMovement> GetByProductId(int productId);
        IEnumerable<InventoryMovement> GetByProductIdOrdered(int productId);  // ← nuevo método
        IEnumerable<InventoryMovement> GetByDateRange(DateTime from, DateTime to);
    }
}