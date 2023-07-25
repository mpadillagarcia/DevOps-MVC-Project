using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.Models
{
    public class PayPal : MetodoPago
    {
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(3, MinimumLength = 2)]
        public string Prefijo { get; set; }

        [StringLength(7, MinimumLength = 6)]
        public string Tlf { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PayPal pal &&
                   base.Equals(obj) &&
                   ID == pal.ID &&
                   Email == pal.Email &&
                   Prefijo == pal.Prefijo &&
                   Tlf == pal.Tlf;
        }

    }
}
