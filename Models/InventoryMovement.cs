using System;
using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Models
{

    public class InventoryMovement
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }           // Cantidad absoluta (siempre positiva)
        public decimal UnitCost { get; private set; }       // Costo unitario
        public decimal TotalCost { get; private set; }      // Costo total (Quantity * UnitCost)
        public MovementType Type { get; private set; }       // Indica si es entrada o salida
        public DateTime Date { get; private set; }           // Fecha del movimiento (UTC)
        public string Description { get; private set; }      // Descripción del movimiento
        public int? RelatedSaleId { get; private set; }      // Id de venta asociada (solo para salidas por venta)

        // Constructor privado sin parámetros (necesario para EF Core o similar, pero aquí lo mantenemos privado)
        private InventoryMovement() { }

        // Constructor público con todos los datos necesarios
        public InventoryMovement(
            int productId,
            int quantity,
            decimal unitCost,
            MovementType type,
            string description,
            int? relatedSaleId = null)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID del producto debe ser un número positivo.");
            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            if (unitCost < 0)
                throw new ArgumentException("El costo unitario no puede ser negativo.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción del movimiento es obligatoria.");

            ProductId = productId;
            Quantity = quantity;
            UnitCost = unitCost;
            TotalCost = quantity * unitCost;   // Cálculo automático
            Type = type;
            Date = DateTime.UtcNow;
            Description = description;
            RelatedSaleId = relatedSaleId;
        }

        // Método controlado para asignar ID desde el repositorio
        public void SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número positivo.");
            Id = id;
        }
    }
}