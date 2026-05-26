using System;
using Sistema_inventario_mvc.Models;

namespace Sistema_inventario_mvc.Models
{
    public class InventoryMovement
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }          // Cantidad absoluta (siempre positiva)
        public MovementType Type { get; private set; }      // Indica si es entrada o salida
        public DateTime Date { get; private set; }           // Fecha del movimiento (UTC)
        public string Description { get; private set; }      // Descripción del movimiento
        public int? RelatedSaleId { get; private set; }      // Id de venta asociada (solo para salidas por venta)

        // Constructor privado sin parámetros (para compatibilidad con inicializadores, aunque no se use)
        private InventoryMovement() { }

        // Constructor público con todos los datos necesarios
        public InventoryMovement(int productId, int quantity, MovementType type, string description, int? relatedSaleId = null)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID del producto debe ser un número positivo.");

            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción del movimiento es obligatoria.");

            ProductId = productId;
            Quantity = quantity;
            Type = type;
            Description = description;
            Date = DateTime.UtcNow;            // Se asigna automáticamente
            RelatedSaleId = relatedSaleId;      // Puede ser null si no proviene de una venta
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