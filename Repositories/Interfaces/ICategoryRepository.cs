using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Category? GetById(int id);
        IEnumerable<Category> GetAll();
        void Add(Category category);
        void Delete(int id);
    }
}