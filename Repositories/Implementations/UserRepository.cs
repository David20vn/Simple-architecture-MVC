using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Helpers;
using Sistema_inventario_mvc.Data;
using Sistema_inventario_mvc.Repositories.Interfaces;

namespace Sistema_inventario_mvc.Repositories.Implementations
{

    public class UserRepository : IUserRepository
    {
        // Referencia directa a la lista en memoria
        private List<User> _users => InMemoryData.Users;
        

        public User? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUsername(string username)  // ← IMPLEMENTADO
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public void Add(User user)
        {
            // Asignar ID incremental simple
            user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
        }

        public void Update(User user)
        {
            var existing = GetById(user.Id);
            if (existing != null)
            {
                existing.Username = user.Username;
                existing.PasswordHash = user.PasswordHash;
                existing.Role = user.Role;
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