using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository; // Para validar categoría

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public Product? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del producto debe ser un número positivo.");

            return _productRepository.GetById(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("El ID de la categoría debe ser un número positivo.");

            // Validar que la categoría existe
            var category = _categoryRepository.GetById(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"No existe una categoría con ID {categoryId}.");

            return _productRepository.GetByCategory(categoryId);
        }

        public IEnumerable<Product> GetAvailable()
        {
            return _productRepository.GetAvailable();
        }

        public IEnumerable<Product> GetLowStock(int threshold)
        {
            if (threshold < 0)
                throw new ArgumentException("El umbral de stock no puede ser negativo.");
            return _productRepository.GetLowStock(threshold);
        
        }
        public Product Create(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("El nombre del producto es obligatorio.");
            if (product.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.");

            var category = _categoryRepository.GetById(product.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"La categoría con ID {product.CategoryId} no existe.");

            _productRepository.Add(product);   // El repositorio asigna el ID automáticamente
            return product;
        }

        
        public Product Update(int id, Product updatedProduct)
        {
            if (updatedProduct == null)
                throw new ArgumentNullException(nameof(updatedProduct));
            if (id <= 0)
                throw new ArgumentException("El ID del producto debe ser un número positivo.");
            if (string.IsNullOrWhiteSpace(updatedProduct.Name))
                throw new ArgumentException("El nombre del producto es obligatorio.");
            if (updatedProduct.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.");

            var existing = _productRepository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");

            var category = _categoryRepository.GetById(updatedProduct.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"La categoría con ID {updatedProduct.CategoryId} no existe.");

            // Uso de métodos encapsulados
            existing.SetName(updatedProduct.Name);
            existing.SetPrice(updatedProduct.Price);
            existing.SetCategory(updatedProduct.CategoryId);

            _productRepository.Update(existing);
            return existing;
        }

        public void UpdatePrice(int id, decimal newPrice)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del producto debe ser un número positivo.");
            if (newPrice <= 0)
                throw new ArgumentException("El nuevo precio debe ser mayor que cero.");

            var product = _productRepository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");

            _productRepository.UpdatePrice(id, newPrice);
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del producto debe ser un número positivo.");

            var product = _productRepository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");

            // No se valida stock ni ventas por requerimiento actual; si se desea, se puede añadir aquí.
            _productRepository.Delete(id);
        }
    }
}