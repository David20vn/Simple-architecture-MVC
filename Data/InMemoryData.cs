using System.Collections.Generic;
using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Data
{
    public static class InMemoryData
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Category> Categories { get; set; } = new List<Category>();
        public static List<Product> Products { get; set; } = new List<Product>();
        public static List<Inventory> Inventories { get; set; } = new List<Inventory>();
        public static List<Sale> Sales { get; set; } = new List<Sale>();
        public static List<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
    }
}