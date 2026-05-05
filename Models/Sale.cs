using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistema_inventario_mvc.Models
{
    public class Sale
    {
        private readonly List<SaleDetail> _details = new();

        public int Id { get; private set; }
        public int UserId { get; private set; }
        public decimal Total => _details.Sum(d => d.SubTotal); // Total calculado automáticamente
        public DateTime SaleDate { get; private set; }        // Fecha de venta
        public IReadOnlyList<SaleDetail> Details => _details.AsReadOnly();

        private Sale() { }

        public Sale(int userId, IEnumerable<SaleDetail> details)
        {
            if (details == null) throw new ArgumentNullException(nameof(details));
            UserId = userId;
            SaleDate = DateTime.UtcNow;
            _details.AddRange(details);
        }

        public void AddDetail(SaleDetail detail)
        {
            if (detail == null) throw new ArgumentNullException(nameof(detail));
            _details.Add(detail);
        }

        public void RemoveDetail(int detailIndex)
        {
            if (detailIndex < 0 || detailIndex >= _details.Count)
                throw new ArgumentException("Índice de detalle inválido.");
            _details.RemoveAt(detailIndex);
        }
    }
}