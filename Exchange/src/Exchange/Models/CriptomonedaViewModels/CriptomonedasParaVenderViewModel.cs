using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.Models.CriptomonedaViewModels
{
    public class CriptomonedasParaVenderViewModel
    {
        public IEnumerable<Criptomoneda> Criptomonedas { get; set; }
        public SelectList Redes;
        [Display(Name = "Red")]
        public string RedMonedaSeleccionada { get; set; }
        [Display(Name = "Nombre")]
        public string NombreMoneda { get; set; }
        [Display(Name = "Capitalizacion")]
        public int Capitalizacion { get; set; }

        [Display(Name = "PorcentajeVariacion")]
        public float PorcentajeVariacion { get; set; }
    }
}