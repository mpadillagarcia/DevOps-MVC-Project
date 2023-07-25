using Exchange.Data;
using Exchange.Models;
using Exchange.UT.CriptomonedasController_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.UT.PrestamosController_test
{
    public static class UtilitiesForPrestamos
    {
        public static void InitializeDbPrestamosForTests(ApplicationDbContext db)
        {
            var prestamos = GetPrestamos(0, 1);
            foreach (Prestamo prestamo in prestamos)
            {
                db.Prestamo.Add(prestamo as Prestamo);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbPrestamosForTests(ApplicationDbContext db)
        {
            db.MonedaPrestada.RemoveRange(db.MonedaPrestada);
            db.Prestamo.RemoveRange(db.Prestamo);
            db.SaveChanges();
        }

        public static IList<Prestamo> GetPrestamos(int index, int numOfPrestamos)
        {

            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            var allPrestamos = new List<Prestamo>();
            Prestamo prestamo;
            Criptomoneda criptomoneda;
            MonedaPrestada prestamoItem;
            int cantidad = 2;

            for (int i = 1; i < 3; i++)
            {
                criptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(i - 1, 1).First();
                //movie.QuantityForPurchase = movie.QuantityForPurchase - quantity;
                prestamo = new Prestamo
                {
                    PrestamoID = i,
                    Cliente = cliente,
                    ClienteId = cliente.Id,
                    MetodoPago = GetMetodoPago(i - 1, 1).First(),
                    FechaPrestamo = System.DateTime.Now,
                    //TotalPrice = movie.PriceForPurchase,
                    MonedasPrestadas = new List<MonedaPrestada>()
                };
                prestamoItem = new MonedaPrestada
                {
                    ID = i,
                    Cantidad = cantidad,
                    Criptomoneda = criptomoneda,
                    CriptomonedaId = criptomoneda.ID,
                    Prestamo = prestamo,
                    PrestamoId = prestamo.PrestamoID

                };
                prestamo.MonedasPrestadas.Add(prestamoItem);
                //purchase.TotalPrice = purchaseItem.Quantity * purchaseItem.Movie.PriceForPurchase;
                allPrestamos.Add(prestamo);

            }

            return allPrestamos.GetRange(index, numOfPrestamos);
        }

        public static IList<MetodoPago> GetMetodoPago(int index, int numOfMetodosPago)
        {
            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            var allMetodosPago = new List<MetodoPago>
                {
                new TarjetaCredito {ID = 1, NumeroTarjeta = "1111111111111111", CVV = "111", FechaCaducidad = new DateTime(2020, 10, 10) },
                new PayPal { ID = 2, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" },

            };
            //return from the list as much instances as specified in numOfRedes
            return allMetodosPago.GetRange(index, numOfMetodosPago);
        }

    }
}
