using System;

namespace Sistema_inventario_mvc.Models
{
    public class SaleDetail
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal SubTotal { get; private set; }

        private SaleDetail() { }

        public SaleDetail(int productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new ArgumentException("Cantidad inválida.");
            if (unitPrice < 0) throw new ArgumentException("Precio unitario inválido.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            SubTotal = quantity * unitPrice;  // Cálculo automático
        }

        public void UpdateQuantity(int newQuantity, decimal currentUnitPrice)
        {
            if (newQuantity <= 0) throw new ArgumentException("Cantidad inválida.");
            Quantity = newQuantity;
            UnitPrice = currentUnitPrice;
            SubTotal = Quantity * UnitPrice;
        }
    }
}