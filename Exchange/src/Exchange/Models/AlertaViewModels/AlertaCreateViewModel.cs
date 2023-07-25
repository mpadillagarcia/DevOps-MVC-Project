using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Exchange.Models.AlertaViewModels
{
    public class AlertaCreateViewModel : IValidatableObject
    {

        public virtual string Nombre
        {
            get;
            set;
        }
        [Display(Name = "Primer Apellido")]
        public virtual string PrimerApellido
        {
            get;
            set;
        }

        [Display(Name = "Segundo Apellido")]
        public virtual string SegundoApellido
        {
            get;
            set;
        }

        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
        public string ClienteId
        {
            get;
            set;
        }


        
        public DateTime FechaAlerta
        {
            get;
            set;
        }

        
        [DataType(DataType.MultilineText)]
        [Display(Name = "FechaExpira")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, selecciona la Fecha de Expiración de la alerta")]

        public DateTime FechaExpira
        {
            get;
            set;
        }

        public virtual IList<AlertaItemViewModel> MonedaAlertar
        {
            get;
            set;
        }


        

        /*
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]

        public String DeliveryAddress
        {
            get;
            set;
        }

        [Display(Name = "Payment Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, select your payment method for delivery")]
        public String PaymentMethod
        {
            get;
            set;
        }


        */


        /*
        [Display(Name = "Credit Card")]
        public virtual string CreditCardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? ExpirationDate { get; set; }

        */

        public AlertaCreateViewModel()
        {

            MonedaAlertar = new List<AlertaItemViewModel>();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is AlertaCreateViewModel model)
                result = Nombre == model.Nombre &&
                  PrimerApellido == model.PrimerApellido &&
                  SegundoApellido == model.SegundoApellido &&
                  ClienteId == model.ClienteId &&
                  FechaAlerta == model.FechaAlerta &&
                  FechaExpira == model.FechaExpira;
            else
                return false;
            for (int i = 0; i < this.MonedaAlertar.Count; i++)
                result = result && (this.MonedaAlertar[i].Equals(model.MonedaAlertar[i]));

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            string mensaje = null;
            //it is checked whether quantity is higher than 0 for at least one movie
            if (MonedaAlertar.Sum(pi => pi.PrecioAlerta) <= MonedaAlertar.Count-1)
            yield return new ValidationResult("Por favor, selecciona un precio de alerta para cada criptomoneda",
            new[] { nameof(MonedaAlertar) });

            /*
            for (int i = 1; i < MonedaAlertar.Count-1; i++)
                if (MonedaAlertar.Sum(pi => pi.PrecioAlerta) <= 1)
                    yield return new ValidationResult("Por favor, selecciona un precio de alerta para la criptomoneda restante",
                         new[] { nameof(MonedaAlertar) });
                else if (MonedaAlertar.Sum(pi => pi.PrecioAlerta) <= MonedaAlertar.Count - 1)
                    mensaje = "Por favor, selecciona un precio de alerta para las " + i + " criptomonedas restantes";
                    yield return new ValidationResult(mensaje, new[] { nameof(MonedaAlertar) });
            */

        }
    }

    public class AlertaItemViewModel
    {
        public virtual int ID
        {
            get;
            set;
        }


        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public virtual String Nombre
        {
            get;
            set;
        }


        [Display(Name = "El campo de precio de alerta es obligatorio")]
        public virtual int Precio
        {
            get;
            set;
        }


        public virtual String NombreRed
        {
            get;
            set;
        }

        [Required]
        public virtual int PrecioAlerta
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            AlertaItemViewModel monedaAlerta = obj as AlertaItemViewModel;
            bool result = false;
            if ((ID == monedaAlerta.ID)
                && (this.Precio == monedaAlerta.Precio)
                && (this.PrecioAlerta == monedaAlerta.PrecioAlerta)
                    && (this.Nombre == monedaAlerta.Nombre)
                    && (this.NombreRed == monedaAlerta.NombreRed))
                result = true;
            return result;
        }

    }
}
