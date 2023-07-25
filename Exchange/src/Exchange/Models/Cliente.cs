using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Exchange.Models
{
    public class Cliente : ApplicationUser 
    {
        public virtual IList<Compra> Compras { get; set; }
        public virtual IList<Prestamo> Prestamos { get; set; }

        public virtual IList<Alerta> Alerta { get; set; }

        public virtual IList<Venta> Ventas { get; set; }
    }
}
