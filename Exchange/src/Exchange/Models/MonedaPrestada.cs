using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exchange.Models
{
    public class MonedaPrestada
    {
        [Key]
        public int ID { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Debes proveer una cantidad válida")]
        public virtual int Cantidad { get; set; }

        [ForeignKey("PrestamoId")]
        public virtual Prestamo Prestamo { get; set; }

        public virtual int PrestamoId { get; set; }

        [ForeignKey("CriptomonedaId")]
        public virtual Criptomoneda Criptomoneda { get; set; }

        public virtual int CriptomonedaId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MonedaPrestada item &&
                   ID == item.ID &&
                   EqualityComparer<Criptomoneda>.Default.Equals(Criptomoneda, item.Criptomoneda) &&
                   Cantidad == item.Cantidad &&
                   CriptomonedaId == item.CriptomonedaId &&
                   EqualityComparer<Prestamo>.Default.Equals(Prestamo, item.Prestamo) &&
                   PrestamoId == item.PrestamoId;
        }

    }
}
