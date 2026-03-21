using InventorySalesAPI.Models;
using InventorySalesAPI.Repositories.Interfaces;
using InventorySalesAPI.Data;
using InventorySalesAPI.Services.Interfaces;

namespace InventorySalesAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Add(Product product)
        {
            // ===== REGLAS DE NEGOCIO =====

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new Exception("El nombre del producto es obligatorio");

            if (product.Price <= 0)
                throw new Exception("El precio debe ser mayor que 0");

            var categoryExists = InMemoryData.Categories.Any(c => c.Id == product.CategoryId);

            if (!categoryExists)
                throw new Exception("La categoría no existe");

            _productRepository.Add(product);
        }

        public void Update(Product product)
        {
            var existing = _productRepository.GetById(product.Id);

            if (existing == null)
                throw new Exception("Producto no encontrado");

            if (product.Price <= 0)
                throw new Exception("Precio inválido");

            _productRepository.Update(product);
        }

        public void Delete(int id)
        {
            var existing = _productRepository.GetById(id);

            if (existing == null)
                throw new Exception("Producto no encontrado");

            _productRepository.Delete(id);
        }

        public Product GetById(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
                throw new Exception("Producto no encontrado");

            return product;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }
    }
}