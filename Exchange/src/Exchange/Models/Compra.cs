using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Exchange.Models
{
    public class Compra
    {
        [Key]
        public virtual int CompraId { get; set; }

        public virtual double PrecioTotal { get; set; }

        public virtual DateTime CompraFecha { get; set; }

        public virtual IList<CompraItem> CompraItems { get; set; }
        public Compra()
        {
            CompraItems = new List<CompraItem>();
        }
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual string ClienteId { get; set; }
        
        [Display(Name = "Metodo de Pago")]
        [Required]
        public virtual MetodoPago MetodoPago { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Compra compra &&
                   CompraId == compra.CompraId &&
                   PrecioTotal == compra.PrecioTotal &&
                   (this.CompraFecha.Subtract(compra.CompraFecha) < new TimeSpan(0, 1, 0)) &&
                   EqualityComparer<Cliente>.Default.Equals(Cliente, compra.Cliente) &&
                   ClienteId == compra.ClienteId;
                   EqualityComparer<MetodoPago>.Default.Equals(MetodoPago, compra.MetodoPago);
        }
    }
}
