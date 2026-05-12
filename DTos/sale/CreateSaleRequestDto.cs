using System.Collections.Generic;

namespace Sistema_inventario_mvc.DTOs
{
    public class CreateSaleRequestDto
    {
        public List<CreateSaleDetailDto> Details { get; set; }
    }
}