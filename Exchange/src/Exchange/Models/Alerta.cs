using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Exchange.Models
{
    public class Alerta
    {
        [Key]
        public virtual int Id { get; set; }

        public DateTime FechaAlerta { get; set; }

        public DateTime FechaExpira { get; set; }

        public virtual IList<MonedaAlerta> MonedaAlertar { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual string ClienteId { get; set; }

        public Alerta()
        {
            MonedaAlertar = new List<MonedaAlerta>();
        }

        public override bool Equals(object obj)
        {
            return obj is Alerta alerta &&
                   Id == alerta.Id &&
                   (this.FechaAlerta.Subtract(alerta.FechaAlerta) < new TimeSpan(0, 1, 0)) &&
                   (this.FechaExpira.Subtract(alerta.FechaExpira) < new TimeSpan(0, 1, 0)) &&
                   EqualityComparer<Cliente>.Default.Equals(Cliente, alerta.Cliente) &&
                   ClienteId == alerta.ClienteId;
        }

    }
}
