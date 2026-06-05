using System.Collections.Generic;

namespace Sistema_inventario_mvc.DTOs
{
    public class ProductKardexResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<KardexRowDto> Rows { get; set; }
    }
}