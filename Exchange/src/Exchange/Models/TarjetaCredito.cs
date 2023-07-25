using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Exchange.Models
{
    public class TarjetaCredito : MetodoPago
    {
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Tienes que introducir 16 números")]
        [Display(Name = "Tarjeta de crédito")]
        [Required]
        public virtual string NumeroTarjeta { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        [Required]
        public virtual string CVV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/yyyy}")]
        public virtual DateTime FechaCaducidad { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TarjetaCredito card &&
                   base.Equals(obj) &&
                   ID == card.ID &&
                   NumeroTarjeta == card.NumeroTarjeta &&
                   CVV == card.CVV &&
                   FechaCaducidad == card.FechaCaducidad;
        }

    }
}