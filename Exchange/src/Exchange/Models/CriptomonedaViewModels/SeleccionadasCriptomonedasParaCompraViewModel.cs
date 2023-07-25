using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.Models.CriptomonedaViewModels
{
    public class SeleccionadasCriptomonedasParaCompraViewModel
    {

        public string[] IdsToAdd { get; set; }
        [Display(Name = "Red")]
        public string criptomonedaRedSeleccionada { get; set; }

        [Display(Name = "Nombre")]
        public string criptomonedaNombre { get; set; }

        [Display(Name = "Precio Moneda")]
        public int Precio { get; set; }
       
        [Display(Name = "PorcentajeVariacion")]
        public float PorcentajeVariacion { get; set; }

    }
}
