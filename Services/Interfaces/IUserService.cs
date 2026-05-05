using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface IUserService
    {
        User? GetById(int id);
        IEnumerable<User> GetAll();
        User Create(User user);
        User Update(int id, User updatedUser);
        void Delete(int id);
    }
}