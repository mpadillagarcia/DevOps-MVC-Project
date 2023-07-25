using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Exchange.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        [Required]
        [StringLength(20, ErrorMessage = "No puede contener más de 20 caracteres.")]
        public virtual string Nombre { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "No puede contener más de 20 caracteres.")]
        public virtual string PrimerApellido { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "No puede contener más de 20 caracteres.")]
        public virtual string SegundoApellido { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaNacimiento { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ApplicationUser user &&
                   Id == user.Id &&
                   Email == user.Email &&
                   PhoneNumber == user.PhoneNumber &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(LockoutEnd, user.LockoutEnd) &&
                   LockoutEnabled == user.LockoutEnabled &&
                   Nombre == user.Nombre &&
                   PrimerApellido == user.PrimerApellido &&
                   SegundoApellido == user.SegundoApellido &&
                   (this.FechaNacimiento.Subtract(user.FechaNacimiento) < new TimeSpan(0, 1, 0));
        }
    }
}