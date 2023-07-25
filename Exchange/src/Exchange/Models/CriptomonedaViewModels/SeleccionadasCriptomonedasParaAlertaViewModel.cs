using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Exchange.Models.CriptomonedaViewModels
{
    public class SeleccionadasCriptomonedasParaAlertaViewModel
    {
        public string[] IdsToAdd { get; set; }
        [Display(Name = "Red")]
        public string criptomonedaRedSeleccionada { get; set; }

        [Display(Name = "Nombre")]
        public string criptomonedaNombre { get; set; }

        [Display(Name = "PorcentajeVariacion")]
        public float porcentajeVariacion { get; set; }

        [Display(Name = "Capitalizacion")]
        public int capitalizacion { get; set; }
    }
}
