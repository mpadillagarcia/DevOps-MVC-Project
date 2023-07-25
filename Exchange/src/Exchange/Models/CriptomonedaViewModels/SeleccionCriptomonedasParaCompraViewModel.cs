using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exchange.Models.CriptomonedaViewModels
{
    public class SeleccionCriptomonedasParaCompraViewModel
    {
        public IEnumerable<Criptomoneda> Criptomonedas { get; set; }
        public SelectList Red;
        
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