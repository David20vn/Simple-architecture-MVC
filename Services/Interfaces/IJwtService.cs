
using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}