using Sistema_inventario_mvc.Data;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;

namespace Sistema_inventario_mvc.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private List<Category> _categories => InMemoryData.Categories;

        public Category? GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _categories;
        }

        public void Add(Category category)
        {
            int newId = _categories.Count > 0 ? _categories.Max(c => c.Id) + 1 : 1;
            category.SetId(newId);
            _categories.Add(category);
        }

        public void Delete(int id)
        {
            var category = GetById(id);
            if (category != null)
                _categories.Remove(category);
        }
    }
}