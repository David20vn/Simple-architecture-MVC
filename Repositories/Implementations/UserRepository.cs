using Sistema_inventario_mvc.Data;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Helpers;

namespace Sistema_inventario_mvc.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users => InMemoryData.Users;

        static UserRepository()
        {
            if (InMemoryData.Users.Count == 0)
            {
                var admin = new User(
                    username: "admin",
                    passwordHash: PasswordHelper.HashPassword("Admin123"),
                    role: Role.Admin
                );
                admin.SetId(1);
                InMemoryData.Users.Add(admin);
            }
        }

        public User? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public void Add(User user)
        {
            // Asignar ID incremental usando el método SetId
            int newId = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            user.SetId(newId);
            _users.Add(user);
        }

        public void Update(User user)
        {
            var existing = GetById(user.Id);
            if (existing != null)
            {
                existing.SetUsername(user.Username);
                existing.SetPasswordHash(user.PasswordHash);
                existing.SetRole(user.Role);
            }
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
                _users.Remove(user);
        }
    }
}