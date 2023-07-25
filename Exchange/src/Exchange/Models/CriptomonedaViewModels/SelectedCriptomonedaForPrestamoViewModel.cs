using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.Models.CriptomonedaViewModels
{
    public class SelectedCriptomonedaForPrestamoViewModel
    {

        public string[] IdsToAdd { get; set; }

        [Display(Name = "Red")]
        public string RedMonedaSeleccionada { get; set; }

        [Display(Name = "Nombre")]
        public string NombreMoneda { get; set; }

        [Display(Name = "Precio")]
        public int Precio { get; set; }

        [Display(Name = "Capitalizacion")]
        public int Capitalizacion { get; set; }

        [Display(Name = "Porcentaje de Variacion")]
        public float PorcentajeVariacion { get; set; }
    }
}
