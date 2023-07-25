using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Models
{
    public class MonedaAlerta
    {
        [Key]
        public virtual int MonedaAlertaID { get; set; }

        [ForeignKey("CriptomonedaId")]
        public virtual Criptomoneda Criptomoneda { get; set; }

        public virtual int ID { get; set; }

        [ForeignKey("AlertaId")]
        public virtual Alerta Alerta { get; set; }

        public virtual int AlertaId { get; set; }

        [Required]
        public virtual string NombreMonedaAlerta { get; set; }

        [Required]
        public virtual int PrecioAlerta { get; set; }


        public override bool Equals(object obj)
        {
            return obj is MonedaAlerta item &&
                   MonedaAlertaID == item.MonedaAlertaID &&
                   EqualityComparer<Criptomoneda>.Default.Equals(Criptomoneda, item.Criptomoneda) &&
                   NombreMonedaAlerta == item.NombreMonedaAlerta &&
                   PrecioAlerta == item.PrecioAlerta &&
                   ID == item.ID &&
                   EqualityComparer<Alerta>.Default.Equals(Alerta, item.Alerta) &&
                   AlertaId == item.AlertaId;
        }


    }
}
