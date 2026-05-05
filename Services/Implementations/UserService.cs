using Sistema_inventario_mvc.Helpers;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Repositories.Interfaces;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.");
            return _userRepository.GetById(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            var existing = _userRepository.GetByUsername(user.Username);
            if (existing != null)
                throw new InvalidOperationException($"El usuario '{user.Username}' ya existe.");

            // Hashear la contraseña usando el método del modelo
            user.SetPasswordHash(PasswordHelper.HashPassword(user.PasswordHash));
            _userRepository.Add(user);
            return user;
        }

        public User Update(int id, User updatedUser)
        {
            if (updatedUser == null)
                throw new ArgumentNullException(nameof(updatedUser));
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            if (string.IsNullOrWhiteSpace(updatedUser.Username))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            var existing = _userRepository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            var other = _userRepository.GetByUsername(updatedUser.Username);
            if (other != null && other.Id != id)
                throw new InvalidOperationException($"El nombre de usuario '{updatedUser.Username}' ya está en uso.");

            existing.SetUsername(updatedUser.Username);
            existing.SetRole(updatedUser.Role);

            if (!string.IsNullOrWhiteSpace(updatedUser.PasswordHash))
            {
                existing.SetPasswordHash(PasswordHelper.HashPassword(updatedUser.PasswordHash));
            }

            _userRepository.Update(existing);
            return existing;
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
            _userRepository.Delete(id);
        }
    }
}