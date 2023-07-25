using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exchange.Models
{
    public class MonedaVendida
    {
        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("CriptomonedaId")]
        public virtual Criptomoneda Criptomoneda { get; set; }

        [Range(1, float.MaxValue, ErrorMessage = "Debe introducir una cantidad válida")]
        public virtual float CantidadVenta { get; set; }

        public virtual int CriptomonedaId { get; set; }

        [ForeignKey("VentaId")]
        public virtual Venta Venta { get; set; }

        public virtual int VentaId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MonedaVendida item &&
                   Id == item.Id &&
                   EqualityComparer<Criptomoneda>.Default.Equals(Criptomoneda, item.Criptomoneda) &&
                   CantidadVenta == item.CantidadVenta &&
                   CriptomonedaId == item.CriptomonedaId &&
                   EqualityComparer<Venta>.Default.Equals(Venta, item.Venta) &&
                   VentaId == item.VentaId;
        }
    }
}