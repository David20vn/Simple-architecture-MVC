using System;

namespace Sistema_inventario_mvc.DTOs
{
    public class KardexRowDto
    {
        // Identificación del movimiento
        public int MovementId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        // Entradas
        public int? EntryQuantity { get; set; }          // Cantidad de entrada (nula si es salida)
        public decimal? EntryUnitCost { get; set; }      // Costo unitario de entrada
        public decimal? EntryTotalCost { get; set; }     // Costo total de entrada

        // Salidas
        public int? ExitQuantity { get; set; }           // Cantidad de salida (nula si es entrada)
        public decimal? ExitUnitCost { get; set; }       // Costo unitario de salida
        public decimal? ExitTotalCost { get; set; }      // Costo total de salida

        // Acumulados
        public int CumulativeEntryQuantity { get; set; } // Acumulado histórico de entradas

        // Saldo después del movimiento
        public int BalanceQuantity { get; set; }         // Stock disponible
        public decimal BalanceAverageCost { get; set; }  // Costo promedio ponderado
        public decimal BalanceTotalValue { get; set; }   // Valor total del inventario
    }
}