using System;

namespace Sistema_inventario_mvc.DTOs
{
    public class InventoryMovementResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }           // "Entry" o "Exit"
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? RelatedSaleId { get; set; }
    }
}