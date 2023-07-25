using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Exchange.Models.VentaViewModels
{
    public class VentaCreateViewModel : IValidatableObject
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

        public double EquivEuros
        {
            get;
            set;
        }

        public DateTime FechaVenta
        {
            get;
            set;
        }

        public virtual IList<VentaItemViewModel> MonedasVendidas
        {
            get;
            set;
        }


        [Display(Name = "Método Pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor selecciona un método de pago")]
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

        public string tlf { get; set; }

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Debes introducir 16 dígitos")]
        [Display(Name = "Tarjeta Crédito")]
        public virtual string NumeroTarjeta { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CVV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? FechaCaducidad { get; set; }

        public VentaCreateViewModel()
        {

            MonedasVendidas = new List<VentaItemViewModel>();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is VentaCreateViewModel model)
                 result = Nombre == model.Nombre &&
                   PrimerApellido == model.PrimerApellido &&
                   SegundoApellido == model.SegundoApellido &&
                   FechaNacimiento == model.FechaNacimiento &&
                   ClienteId == model.ClienteId &&
                   EquivEuros == model.EquivEuros &&
                   FechaVenta == model.FechaVenta &&
                   MetodoPago == model.MetodoPago &&
                   Email == model.Email &&
                   //Prefijo == model.Prefijo && Alejandro Moya: error desconocido
                   tlf == model.tlf &&
                   NumeroTarjeta == model.NumeroTarjeta &&
                   CVV == model.CVV &&
                   FechaCaducidad == model.FechaCaducidad;
            else
                return false;
            for (int i = 0; i < this.MonedasVendidas.Count; i++)
                result = result && (this.MonedasVendidas[i].Equals(model.MonedasVendidas[i]));

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MetodoPago == "TarjetaCredito")
            {
                if (NumeroTarjeta == null)
                    yield return new ValidationResult("Rellena el campo de 'Número de Tarjeta'",
                        new[] { nameof(NumeroTarjeta) });
                if (CVV == null)
                    yield return new ValidationResult("Rellena el campo 'CVV'",
                        new[] { nameof(CVV) });
                if (FechaCaducidad == null)
                    yield return new ValidationResult("Rellena el campo 'Fecha de Caducidad'",
                        new[] { nameof(FechaCaducidad) });
            }
            else
            {
                if (Email == null)
                    yield return new ValidationResult("Rellena el campo 'Email'",
                        new[] { nameof(Email) });
                if (Prefijo == null)
                    yield return new ValidationResult("Rellena el campo 'Prefijo'",
                        new[] { nameof(Prefijo) });
                if (tlf == null)
                    yield return new ValidationResult("Rellena el campo 'Teléfono'",
                        new[] { nameof(tlf) });
            }

            //it is checked whether quantity is higher than 0 for at least one movie
            if (MonedasVendidas.Sum(pi => pi.CantidadAVender) <= 0)
                yield return new ValidationResult("Selecciona una cantidad",
                     new[] { nameof(MonedasVendidas) });



        }
    }

    public class VentaItemViewModel
    {
        public virtual int ID
        {
            get;
            set;
        }


        [StringLength(50, ErrorMessage = "No puede tener más de 50 caracteres.")]
        public virtual String Nombre
        {
            get;
            set;
        }


        [Display(Name = "Precio venta")]
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

        [Required]
        public virtual int CantidadAVender
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            VentaItemViewModel monedaVendida = obj as VentaItemViewModel;
            bool result = false;
            if ((ID == monedaVendida.ID)
                && (this.Precio == monedaVendida.Precio)  
 
                    && (this.CantidadAVender == monedaVendida.CantidadAVender)
                    && (this.Nombre == monedaVendida.Nombre)
                    && (this.PorcentajeVariacion == monedaVendida.PorcentajeVariacion)
                    && (this.Capitalizacion == monedaVendida.Capitalizacion))

                result = true;
            return result;
        }
    }
}