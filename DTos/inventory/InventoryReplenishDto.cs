namespace Sistema_inventario_mvc.DTOs
{
    public class InventoryReplenishDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }
}