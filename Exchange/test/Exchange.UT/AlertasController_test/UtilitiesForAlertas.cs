using Exchange.Data;
using Exchange.Models;
using Exchange.UT.CriptomonedasController_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.UT.AlertasController_test
{
    class UtilitiesForAlertas
    {
        public static void InitializeDbAlertasForTests(ApplicationDbContext db)
        {
            var alertas = GetAlertas(0, 1);
            foreach (Alerta alerta in alertas)
            {
                db.Alerta.Add(alerta as Alerta);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbAlertasForTests(ApplicationDbContext db)
        {
            db.MonedaAlerta.RemoveRange(db.MonedaAlerta);
            db.Alerta.RemoveRange(db.Alerta);
            db.SaveChanges();
        }

        public static IList<Alerta> GetAlertas(int index, int numOfAlertas)
        {

            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            var allAlertas = new List<Alerta>();
            Alerta alerta;
            Criptomoneda criptomoneda;
            MonedaAlerta monedaAlerta;
            int PrecioAlerta = 2;

            for (int i = 1; i < 3; i++)
            {
                criptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(i - 1, 1).First();
                criptomoneda.CantidadAComprar = criptomoneda.CantidadAComprar - PrecioAlerta;
                alerta = new Alerta
                {
                    Id = i,
                    Cliente = cliente,
                    ClienteId = cliente.Id,
                    FechaAlerta = System.DateTime.Now,
                    FechaExpira = System.DateTime.Now.AddDays(2),
                    MonedaAlertar = new List<MonedaAlerta>()
                };
                monedaAlerta = new MonedaAlerta
                {
                    MonedaAlertaID = i,
                    PrecioAlerta = PrecioAlerta,
                    Criptomoneda = criptomoneda,
                    NombreMonedaAlerta = criptomoneda.Nombre,
                    Alerta = alerta,
                    AlertaId = alerta.Id

                };
                alerta.MonedaAlertar.Add(monedaAlerta);
                
                allAlertas.Add(alerta);

            }

            return allAlertas.GetRange(index, numOfAlertas);
        }

        /*
        public static IList<PaymentMethod> GetPaymentMethod(int index, int numOfPaymentMethods)
        {
            Customer customer = Utilities.GetUsers(0, 1).First() as Customer;
            var allPaymentMethods = new List<PaymentMethod>
                {
                new CreditCard {ID = 1, CreditCardNumber = "1111111111111111", CCV = "111", ExpirationDate = new DateTime(2020, 10, 10) },
                new PayPal { ID = 2, Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" },

            };
            //return from the list as much instances as specified in numOfGenres
            return allPaymentMethods.GetRange(index, numOfPaymentMethods);
        } */
    }
}
