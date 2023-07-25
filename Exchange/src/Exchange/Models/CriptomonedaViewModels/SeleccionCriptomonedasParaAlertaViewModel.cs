using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;




namespace Exchange.Models.CriptomonedaViewModels
{
    public class SeleccionCriptomonedasParaAlertaViewModel
    {
        public IEnumerable<Criptomoneda> Criptomonedas { get; set; }

        public SelectList Red;
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