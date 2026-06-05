using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<Inventory> GetAll();
        Inventory? GetById(int id);
        Inventory? GetByProductId(int productId);
        Inventory AddStock(int productId, int quantity, decimal unitCost);
        Inventory SubtractStock(int productId, int quantity, int? saleId = null);
    }
}