using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetById(int id);
        User? GetByUsername(string username);
        List<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}