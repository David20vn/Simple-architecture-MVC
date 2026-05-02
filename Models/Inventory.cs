
namespace Sistema_inventario_mvc.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }

        public Inventory(){}
        public Inventory(int productId, int stockQuantity)
        {
            ProductId = productId;
            StockQuantity = stockQuantity;
        }
    }
}