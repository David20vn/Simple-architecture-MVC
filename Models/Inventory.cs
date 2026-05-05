using System;

namespace Sistema_inventario_mvc.Models
{
    public class Inventory
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int StockQuantity { get; private set; }

        private Inventory() { }

        public Inventory(int productId, int initialStock)
        {
            if (initialStock < 0)
                throw new ArgumentException("El stock inicial no puede ser negativo.");
            ProductId = productId;
            StockQuantity = initialStock;
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a agregar debe ser positiva.");
            StockQuantity += quantity;
        }

        public void SubtractStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a restar debe ser positiva.");
            if (StockQuantity < quantity)
                throw new InvalidOperationException("No hay suficiente stock disponible.");
            StockQuantity -= quantity;
        }
    }
}