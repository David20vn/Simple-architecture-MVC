using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface ISaleService
    {
        Sale CreateSale(int userId, List<SaleDetail> details);
        Sale? GetById(int id);
        IEnumerable<Sale> GetAll();
        IEnumerable<Sale> GetByDateRange(DateTime from, DateTime to);
    }
}