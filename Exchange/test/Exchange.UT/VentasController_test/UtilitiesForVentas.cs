using Exchange.Data;
using Exchange.Models;
using Exchange.UT.CriptomonedasController_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.UT.VentasController_test
{
    public static class UtilitiesForVentas
    {
        public static void InitializeDbVentasForTests(ApplicationDbContext db)
        {
            var ventas = GetVentas(0, 1);
            foreach (Venta venta in ventas)
            {
                db.Venta.Add(venta as Venta);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbVentasForTests(ApplicationDbContext db)
        {
            db.MonedaVendida.RemoveRange(db.MonedaVendida);
            db.Venta.RemoveRange(db.Venta);
            db.SaveChanges();
        }

        public static IList<Venta> GetVentas(int index, int numOfVentas)
        {

            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            var allVentas = new List<Venta>();
            Venta venta;
            Criptomoneda criptomoneda;
            MonedaVendida monedavendida;
            int cantidad = 2;

            for (int i = 1; i < 3; i++)
            {
                criptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
                criptomoneda.CantidadAVender = criptomoneda.CantidadAVender - cantidad;
                venta = new Venta
                {
                    VentaId = i,
                    Cliente = cliente,
                    ClienteId = cliente.Id,
                    MetodoPago = GetMetodoPago(i - 1, 1).First(),
                    FechaVenta = DateTime.Now,
                    EquivEuros = criptomoneda.Precio,
                    MonedasVendidas = new List<MonedaVendida>()
                };
                monedavendida = new MonedaVendida
                {
                    Id = i,
                    CantidadVenta = cantidad,
                    Criptomoneda = criptomoneda,
                    CriptomonedaId = criptomoneda.ID,
                    Venta = venta,
                    VentaId = venta.VentaId

                };
                venta.MonedasVendidas.Add(monedavendida);
                venta.EquivEuros = monedavendida.CantidadVenta * monedavendida.Criptomoneda.Precio;
                allVentas.Add(venta);

            }

            return allVentas.GetRange(index, numOfVentas);
        }

        public static IList<MetodoPago> GetMetodoPago(int index, int numOfMetodosPago)
        {
            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            var allMetodosPago = new List<MetodoPago>
                {
                new TarjetaCredito {ID = 1, NumeroTarjeta = "1111111111111111", CVV = "111", FechaCaducidad = new DateTime(2020, 10, 10) },
                new PayPal { ID = 2, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" },

            };
            //return from the list as much instances as specified in numOfGenres
            return allMetodosPago.GetRange(index, numOfMetodosPago);
        }

    }
}