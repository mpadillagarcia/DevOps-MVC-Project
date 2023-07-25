using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exchange.Models
{
    public class Prestamo
    {
        [Key]
        public virtual int PrestamoID { get; set; }

        public DateTime FechaPrestamo { get; set; }

        public virtual int TasaInteres { get; set; }

        [Required()]
        public virtual MetodoPago MetodoPago { get; set; }

        public virtual IList<MonedaPrestada> MonedasPrestadas { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual string ClienteId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Prestamo prestamo &&
                   PrestamoID == prestamo.PrestamoID &&
                   TasaInteres == prestamo.TasaInteres &&
                   (this.FechaPrestamo.Subtract(prestamo.FechaPrestamo) < new TimeSpan(0, 1, 0)) &&
                   EqualityComparer<Cliente>.Default.Equals(Cliente, prestamo.Cliente) &&
                   ClienteId == prestamo.ClienteId &&
                   EqualityComparer<MetodoPago>.Default.Equals(MetodoPago, prestamo.MetodoPago);
        }
    }
}
