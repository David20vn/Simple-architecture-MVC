using System;

namespace Sistema_inventario_mvc.Models
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private Category() { }

        public Category(string name)
        {
            SetName(name);
        }

        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
            Name = newName;
        }

        public void SetId(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            Id = id;
        }
    }
}