using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Category? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la categoría debe ser un número positivo.");
            return _categoryRepository.GetById(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category Create(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            // Validar que no exista una categoría con el mismo nombre
            var existing = _categoryRepository.GetAll()
                .FirstOrDefault(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
                throw new InvalidOperationException($"Ya existe una categoría con el nombre '{category.Name}'.");

            _categoryRepository.Add(category);
            return category;
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la categoría debe ser un número positivo.");

            var category = _categoryRepository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"Categoría con ID {id} no encontrada.");

            _categoryRepository.Delete(id);
        }
    }
}