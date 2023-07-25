using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace Exchange.Models
{
    public class CompraItem
    {
        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("CriptomonedaId")]
        public virtual Criptomoneda Criptomoneda { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Debes introducir una cantidad valida")]
        public virtual int Cantidad { get; set; }

        public virtual int CriptomonedaId { get; set; }

        [ForeignKey("CompraId")]
        public virtual Compra Compra { get; set; }

        public virtual int CompraId { get; set; }
        public override bool Equals(object obj)
        {
            return obj is CompraItem item &&
                   Id == item.Id &&
                   EqualityComparer<Criptomoneda>.Default.Equals(Criptomoneda, item.Criptomoneda) &&
                   Cantidad == item.Cantidad &&
                   CriptomonedaId == item.CriptomonedaId &&
                   EqualityComparer<Compra>.Default.Equals(Compra, item.Compra) &&
                   CompraId == item.CompraId;
        }
    }

}