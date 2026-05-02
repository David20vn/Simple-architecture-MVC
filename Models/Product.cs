namespace Sistema_inventario_mvc.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        
        public Product(){}
        public Product(string name, decimal price, int categoryId)
        {
            Name = name;
            Price = price;
            CategoryId = categoryId;
        }
    }
}