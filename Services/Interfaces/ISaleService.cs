using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface ISaleService
    {
        void CreateSale(Sale sale);
        Sale GetById(int id);
        List<Sale> GetAll();
    }
}