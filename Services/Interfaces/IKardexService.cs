using Sistema_inventario_mvc.DTOs;

namespace Sistema_inventario_mvc.Services.Interfaces
{
    public interface IKardexService
    {
        ProductKardexResponseDto GetKardex(int productId, DateTime? from = null, DateTime? to = null);
    }
}