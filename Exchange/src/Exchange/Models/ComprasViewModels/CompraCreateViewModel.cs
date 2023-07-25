using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Exchange.Models.CompraViewModels
{
    public class CompraCreateViewModel : IValidatableObject
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

        public double PrecioTotal
        {
            get;
            set;
        }

        public DateTime FechaCompra
        {
            get;
            set;
        }

        public virtual IList<CompraItemViewModel> CompraItems
        {
            get;
            set;
        }


        [Display(Name = "Metodo De Pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, selecciona un metodo de pago")]
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

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Debes introducir 16 numeros")]
        [Display(Name = "Tarjeta de credito")]
        public virtual string NumeroTarjeta { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CVV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? FechaExpiracion { get; set; }

        public CompraCreateViewModel()
        {

            CompraItems = new List<CompraItemViewModel>();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is CompraCreateViewModel model)
                result = Nombre == model.Nombre &&
                  PrimerApellido == model.PrimerApellido &&
                  SegundoApellido == model.SegundoApellido &&
                  ClienteId == model.ClienteId &&
                  PrecioTotal == model.PrecioTotal &&
                  FechaCompra == model.FechaCompra &&
                  MetodoPago == model.MetodoPago &&
                  Email == model.Email &&
                  Prefijo == model.Prefijo &&
                  Tlf == model.Tlf &&
                  NumeroTarjeta == model.NumeroTarjeta &&
                  CVV == model.CVV &&
                  FechaExpiracion == model.FechaExpiracion;
            else
                return false;
            for (int i = 0; i < this.CompraItems.Count; i++)
                result = result && (this.CompraItems[i].Equals(model.CompraItems[i]));

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MetodoPago == "TarjetaCredito")
            {
                if (NumeroTarjeta == null)
                    yield return new ValidationResult("Porfavor, Rellena el campo Numero de Tarjeta",
                        new[] { nameof(NumeroTarjeta) });
                if (CVV == null)
                    yield return new ValidationResult("Porfavor, Rellena el campo CVV",
                        new[] { nameof(CVV) });
                if (FechaExpiracion == null)
                    yield return new ValidationResult("Porfavor, Rellena el campo Fecha de expiracion",
                        new[] { nameof(FechaExpiracion) });
            }
            else
            {
                if (Email == null)
                    yield return new ValidationResult("Porfavor, Rellena el campo Email",
                        new[] { nameof(Email) });
                if (Prefijo == null)
                    yield return new ValidationResult("Porfavor, Rellena el campo Prefijo",
                        new[] { nameof(Prefijo) });
                if (Tlf == null)
                    yield return new ValidationResult("Porfavor, Rellena el campo Tlf",
                        new[] { nameof(Tlf) });
            }

            //it is checked whether quantity is higher than 0 for at least one movie
            if (CompraItems.Sum(pi => pi.Cantidad) <= 0)
                yield return new ValidationResult("Porfavor, Selecciona una cantidad mayor que 0",
                     new[] { nameof(CompraItems) });



        }
    }

    public class CompraItemViewModel
    {
        public virtual int ID
        {
            get;
            set;
        }


        [StringLength(50, ErrorMessage = "No puede ser mayor de 50 caracteres")]
        public virtual String Nombre
        {
            get;
            set;
        }


        [Display(Name = "Precio de Compra")]
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

        [Required]
        public virtual int Cantidad
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            CompraItemViewModel compraItem = obj as CompraItemViewModel;
            bool result = false;
            if ((ID == compraItem.ID)
                && (this.Precio == compraItem.Precio)
                    && (this.Cantidad == compraItem.Cantidad)
                    && (this.Nombre == compraItem.Nombre))
                result = true;
            return result;
        }
    }
}