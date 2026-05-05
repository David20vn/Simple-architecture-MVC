using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface ICategoryService
    {
        Category? GetById(int id);
        IEnumerable<Category> GetAll();
        Category Create(Category category);
        void Delete(int id);
    }
}