using Exchange.Data;
using Exchange.Models;
using Exchange.UT.CriptomonedasController_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.UT.ComprasController_test
{
    public static class UtilitiesForCompras
    {
        public static void InitializeDbComprasForTests(ApplicationDbContext db)
        {
            var compras = GetCompras(0, 1);
            foreach (Compra compra in compras)
            {
                db.Compra.Add(compra as Compra);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbComprasForTests(ApplicationDbContext db)
        {
            db.CompraItem.RemoveRange(db.CompraItem);
            db.Compra.RemoveRange(db.Compra);
            db.SaveChanges();
        }

        public static IList<Compra> GetCompras(int index, int numOfCompras)
        {

            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allCompras = new List<Compra>();
            Compra compra;
            Criptomoneda criptomoneda;
            CompraItem compraItem;
            int cantidad = 2;

            for (int i = 1; i < 3; i++)
            {
                criptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
                criptomoneda.CantidadAComprar = criptomoneda.CantidadAComprar - cantidad;
                compra = new Compra
                {
                    CompraId = i,
                    Cliente = customer,
                    ClienteId = customer.Id,
                    MetodoPago = GetMetodoPago(i - 1, 1).First(),
                    CompraFecha = System.DateTime.Now,
                    PrecioTotal = criptomoneda.Precio,
                    CompraItems = new List<CompraItem>()
                };
                compraItem = new CompraItem
                {
                    Id = i,
                    Cantidad = cantidad,
                    Criptomoneda = criptomoneda,
                    CriptomonedaId = criptomoneda.ID,
                    Compra = compra,
                    CompraId = compra.CompraId

                };
                compra.CompraItems.Add(compraItem);
                compra.PrecioTotal = compraItem.Cantidad * compraItem.Criptomoneda.Precio;
                allCompras.Add(compra);

            }

            return allCompras.GetRange(index, numOfCompras);
        }

        public static IList<MetodoPago> GetMetodoPago(int index, int numOfMetodosPagos)
        {
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allMetodosPago = new List<MetodoPago>
                {
                new TarjetaCredito {ID = 1, NumeroTarjeta = "1111111111111111", CVV = "111", FechaCaducidad = new DateTime(2020, 10, 10) },
                new PayPal { ID = 2, Email = customer.Email, Tlf = customer.PhoneNumber, Prefijo = "+34" },

            };
            //return from the list as much instances as specified in numOfGenres
            return allMetodosPago.GetRange(index, numOfMetodosPagos);
        }

    }
}