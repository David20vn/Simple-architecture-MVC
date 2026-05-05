using System;

namespace Sistema_inventario_mvc.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public Role Role { get; private set; }

        // Constructor vacío necesario para algunas operaciones (se mantiene)
        private User() { }

        public User(string username, string passwordHash, Role role)
        {
            SetUsername(username);
            PasswordHash = passwordHash; // El hash ya viene calculado desde el servicio
            Role = role;
        }

        public void SetUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            Username = newUsername;
        }

        // En el servicio, al crear/actualizar se llamará a este método
        public void SetPasswordHash(string newHash)
        {
            if (string.IsNullOrWhiteSpace(newHash))
                throw new ArgumentException("El hash no puede estar vacío.");
            PasswordHash = newHash;
        }

        public void SetRole(Role newRole)
        {
            if (!Enum.IsDefined(typeof(Role), newRole))
                throw new ArgumentException("Rol inválido.");
            Role = newRole;
        }

        public void SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.");
            Id = id;
        }
    }
}