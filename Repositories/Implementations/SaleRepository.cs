using Sistema_inventario_mvc.Data;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;

namespace Sistema_inventario_mvc.Repositories.Implementations
{
    public class SaleRepository : ISaleRepository
    {
        private List<Sale> _sales => InMemoryData.Sales;

        public Sale? GetById(int id)
        {
            return _sales.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Sale> GetAll()
        {
            return _sales;
        }

        public IEnumerable<Sale> GetByDateRange(DateTime from, DateTime to)
        {
            return _sales.Where(s => s.SaleDate >= from && s.SaleDate <= to);
        }

        public void Add(Sale sale)
        {
            int newId = _sales.Count > 0 ? _sales.Max(s => s.Id) + 1 : 1;
            sale.SetId(newId);
            _sales.Add(sale);
        }

        public void Delete(int id)
        {
            var sale = GetById(id);
            if (sale != null)
                _sales.Remove(sale);
        }
    }
}