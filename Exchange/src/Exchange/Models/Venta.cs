using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exchange.Models
{
    public class Venta
    {
        [Key]
        public virtual int VentaId { get; set; }

        public virtual float CantidadVendida { get; set; }

        public virtual float EquivEuros { get; set; }

        public virtual DateTime FechaVenta { get; set; }

        public virtual IList<MonedaVendida> MonedasVendidas { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual string ClienteId {get; set;}

        [Display(Name = "Metodo de Pago")]
        [Required]
        public MetodoPago MetodoPago { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Venta venta &&
                   VentaId == venta.VentaId &&
                   CantidadVendida == venta.CantidadVendida &&
                   EquivEuros == venta.EquivEuros &&
                   (this.FechaVenta.Subtract(venta.FechaVenta) < new TimeSpan(0, 1, 0)) &&
                   EqualityComparer<Cliente>.Default.Equals(Cliente, venta.Cliente) &&
                   ClienteId == venta.ClienteId;
                   EqualityComparer<MetodoPago>.Default.Equals(MetodoPago, venta.MetodoPago);
        }
    }
}