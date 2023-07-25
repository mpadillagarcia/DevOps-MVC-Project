using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Exchange.Models
{
    public class MetodoPago
    {
        [Key]
        public virtual int ID { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MetodoPago method &&
                   ID == method.ID;
        }

    }

}