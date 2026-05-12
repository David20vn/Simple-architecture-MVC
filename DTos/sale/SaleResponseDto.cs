using System;
using System.Collections.Generic;

namespace Sistema_inventario_mvc.DTOs
{
    public class SaleResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SaleDetailResponseDto> Details { get; set; }
    }
}