using InventorySalesAPI.Models;

namespace InventorySalesAPI.Services.Interfaces
{
    public interface ISaleService
    {
        void CreateSale(Sale sale);
        Sale GetById(int id);
        List<Sale> GetAll();
    }
}