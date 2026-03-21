using System;
using System.Collections.Generic;
using InventorySalesAPI.Models;

namespace InventorySalesAPI.Data
{
    public static class InMemoryData
    {
        // =========================
        // ROLES
        // =========================
        public static List<Role> Roles = new List<Role>()
        {
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Seller" }
        };

        // =========================
        // USERS
        // =========================
        public static List<User> Users = new List<User>()
        {
            new User
            {
                Id = 1,
                Username = "admin",
                Password = "123",
                RoleId = 1
            },
            new User
            {
                Id = 2,
                Username = "seller",
                Password = "123",
                RoleId = 2
            }
        };

        // =========================
        // CATEGORIES
        // =========================
        public static List<Category> Categories = new List<Category>()
        {
            new Category { Id = 1, Name = "Beverages" },
            new Category { Id = 2, Name = "Electronics" }
        };

        // =========================
        // PRODUCTS
        // =========================
        public static List<Product> Products = new List<Product>()
        {
            new Product
            {
                Id = 1,
                Name = "Coca Cola",
                Price = 4000,
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Headphones",
                Price = 80000,
                CategoryId = 2
            }
        };

        // =========================
        // INVENTORIES
        // =========================
        public static List<Inventory> Inventories = new List<Inventory>()
        {
            new Inventory
            {
                Id = 1,
                ProductId = 1,
                StockQuantity = 50
            },
            new Inventory
            {
                Id = 2,
                ProductId = 2,
                StockQuantity = 20
            }
        };

        // =========================
        // SALES
        // =========================
        public static List<Sale> Sales = new List<Sale>()
        {
            new Sale
            {
                Id = 1,
                Date = DateTime.Now,
                UserId = 2,
                Total = 4000,
                Details = new List<SaleDetail>()
                {
                    new SaleDetail
                    {
                        Id = 1,
                        ProductId = 1,
                        Quantity = 1,
                        UnitPrice = 4000
                    }
                }
            }
        };

        // =========================
        // HELPERS (ID GENERATORS)
        // =========================

        public static int GetNextUserId()
        {
            return Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
        }

        public static int GetNextCategoryId()
        {
            return Categories.Any() ? Categories.Max(c => c.Id) + 1 : 1;
        }

        public static int GetNextProductId()
        {
            return Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
        }

        public static int GetNextInventoryId()
        {
            return Inventories.Any() ? Inventories.Max(i => i.Id) + 1 : 1;
        }

        public static int GetNextSaleId()
        {
            return Sales.Any() ? Sales.Max(s => s.Id) + 1 : 1;
        }

        public static int GetNextSaleDetailId(List<SaleDetail> details)
        {
            return details.Any() ? details.Max(d => d.Id) + 1 : 1;
        }
    }
}