using System;

namespace Sistema_inventario_mvc.Models
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int CategoryId { get; private set; }

        private Product() { }

        public Product(string name, decimal price, int categoryId)
        {
            SetName(name);
            SetPrice(price);
            CategoryId = categoryId; // Se valida existencia en el servicio
        }

        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("El nombre del producto no puede estar vacío.");
            Name = newName;
        }

        public void SetPrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.");
            Price = newPrice;
        }

        public void SetCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("El ID de categoría no es válido.");
            CategoryId = categoryId;
        }

        public void SetId(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            Id = id;
        }
    }
}