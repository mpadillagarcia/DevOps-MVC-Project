using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Exchange.Models
{
    public class Red
    {
        [Key]
        public virtual int RedID { get; set; }

        public virtual string nombre { get; set; }

        public virtual ICollection<Criptomoneda> Criptomonedas { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Red red &&
                   RedID == red.RedID &&
                   nombre == red.nombre;
        }
    }
}