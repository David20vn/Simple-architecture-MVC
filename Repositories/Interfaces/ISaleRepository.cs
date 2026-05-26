using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        Sale? GetById(int id);
        IEnumerable<Sale> GetAll();
        IEnumerable<Sale> GetByDateRange(DateTime from, DateTime to);
        void Add(Sale sale);
        void Delete(int id);
    }
}