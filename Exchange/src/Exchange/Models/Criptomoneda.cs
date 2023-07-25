using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Exchange.Models
{
    public class Criptomoneda
    {
        [Key]
        public virtual int ID { get; set; }

        [Required]
        public virtual string Nombre { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El precio mínimo es de 1 euro")]
        public virtual int Precio { get; set; }

        [Required]
        public virtual float PorcentajeVariacion { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La capitalización no puede ser menor que 0")]
        public virtual int Capitalizacion { get; set; }

        [Required]
        public virtual int NombreRed { get; set; }

        public virtual Red Red { get; set; }

        [Required]
        [Display(Name = "Cantidad a Comprar")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad minima de compra es 1")]
        public virtual int CantidadAComprar { get; set; }
        public virtual IList<MonedaPrestada> MonedaPrestada { get; set; }
        [Display(Name = "Cantidad a Vender")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio mínimo es de 1 euro")]
        public virtual int CantidadAVender { get; set; }

        public virtual IList<CompraItem> MonedasCompradas { get; set; }
        public virtual IList<MonedaAlerta> MonedaAlertas { get; set; }
        public virtual IList<MonedaVendida> MonedasVendidas { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Criptomoneda criptomoneda &&
                   ID == criptomoneda.ID &&
                   Nombre == criptomoneda.Nombre &&
                   Precio == criptomoneda.Precio &&
                   PorcentajeVariacion == criptomoneda.PorcentajeVariacion &&
                   Capitalizacion == criptomoneda.Capitalizacion &&
                   NombreRed == criptomoneda.NombreRed &&
                   EqualityComparer<Red>.Default.Equals(Red, criptomoneda.Red) &&
                   CantidadAComprar == criptomoneda.CantidadAComprar &&
                   CantidadAVender == criptomoneda.CantidadAVender;
        }
    }
}