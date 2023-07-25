using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Exchange.Models.PrestamoViewModels
{
    public class PrestamoCreateViewModel : IValidatableObject
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

        [Display(Name = "Fecha de nacimiento")]
        public virtual string FechaNacimiento
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

        public DateTime FechaPrestamo
        {
            get;
            set;
        }

        public virtual IList<PrestamoItemViewModel> PrestamoItems
        {
            get;
            set;
        }

        [Display(Name = "Método de pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, select your payment method for delivery")]
        public String MetodoPago
        {
            get;
            set;
        }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(3, MinimumLength = 2)]
        public string Prefijo { get; set; }


        [StringLength(7, MinimumLength = 6)]

        public string Tlf { get; set; }

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "You have to introduce 16 numbers")]
        [Display(Name = "Tarjeta de crédito")]
        public virtual string NumeroTarjeta { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CVV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? FechaCaducidad { get; set; }

        public PrestamoCreateViewModel()
        {

            PrestamoItems = new List<PrestamoItemViewModel>();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is PrestamoCreateViewModel model)
                result = Nombre == model.Nombre &&
                  PrimerApellido == model.PrimerApellido &&
                  SegundoApellido == model.SegundoApellido &&
                  ClienteId == model.ClienteId &&
                  FechaPrestamo == model.FechaPrestamo &&
                  MetodoPago == model.MetodoPago &&
                  Email == model.Email &&
                  Prefijo == model.Prefijo &&
                  Tlf == model.Tlf &&
                  NumeroTarjeta == model.NumeroTarjeta &&
                  CVV == model.CVV &&
                  FechaCaducidad == model.FechaCaducidad;
            else
                return false;
            for (int i = 0; i < this.PrestamoItems.Count; i++)
                result = result && (this.PrestamoItems[i].Equals(model.PrestamoItems[i]));

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MetodoPago == "TarjetaCredito")
            {
                if (NumeroTarjeta == null)
                    yield return new ValidationResult("Please, fill in your Credit Card Number for your Credit Card payment",
                        new[] { nameof(NumeroTarjeta) });
                if (CVV == null)
                    yield return new ValidationResult("Please, fill in your CCV for your Credit Card payment",
                        new[] { nameof(CVV) });
                if (FechaCaducidad == null)
                    yield return new ValidationResult("Please, fill in your ExpirationDate for your Credit Card payment",
                        new[] { nameof(FechaCaducidad) });
            }
            else
            {
                if (Email == null)
                    yield return new ValidationResult("Please, fill in your Email for your PayPal payment",
                        new[] { nameof(Email) });
                if (Prefijo == null)
                    yield return new ValidationResult("Please, fill in your Prefix for your PayPal payment",
                        new[] { nameof(Prefijo) });
                if (Tlf == null)
                    yield return new ValidationResult("Please, fill in your Phone for your PayPal payment",
                        new[] { nameof(Tlf) });
            }

            //it is checked whether quantity is higher than 0 for at least one cripto
            if (PrestamoItems.Sum(pi => pi.Cantidad) <= 0)
                yield return new ValidationResult("Please, select Quantity higher than 0 for at least one criptomoneda",
                     new[] { nameof(PrestamoItems) });



        }
    }

    public class PrestamoItemViewModel
    {
        public virtual int CriptomonedaId
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


        [Display(Name = "Precio")]
        public virtual int Precio
        {
            get;
            set;
        }


        public virtual String Red
        {
            get;
            set;
        }

        public virtual float PorcentajeVariacion
        {
            get;
            set;
        }

        public virtual int Capitalizacion
        {
            get;
            set;
        }

        [Required(ErrorMessage = "The Quantity field is required")]
        public virtual int Cantidad
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            PrestamoItemViewModel prestamoItem = obj as PrestamoItemViewModel;
            bool result = false;
            if ((CriptomonedaId == prestamoItem.CriptomonedaId)
                && (this.Precio == prestamoItem.Precio)
                    && (this.Cantidad == prestamoItem.Cantidad)
                    && (this.Nombre == prestamoItem.Nombre)
                    && (this.PorcentajeVariacion == prestamoItem.PorcentajeVariacion)
                    && (this.Capitalizacion == prestamoItem.Capitalizacion))
                result = true;
            return result;
        }
    }
}
