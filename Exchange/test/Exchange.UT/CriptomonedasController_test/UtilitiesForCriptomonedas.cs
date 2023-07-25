using Exchange.Data;
using Exchange.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.UT.CriptomonedasController_test
{
    public static class UtilitiesForCriptomonedas
    {


        public static void InitializeDbRedesForTests(ApplicationDbContext db)
        {
            db.Red.AddRange(GetRedes(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbRedesForTests(ApplicationDbContext db)
        {
            db.Red.RemoveRange(db.Red);
            db.SaveChanges();
        }

        public static void InitializeDbCriptomonedasForTests(ApplicationDbContext db)
        {

            db.Criptomoneda.AddRange(GetCriptomonedas(0, 4));
            //genre id=1 it is already added because it is related to the movies


            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });
            db.SaveChanges();
        }

        public static void ReInitializeDbCriptomonedasForTests(ApplicationDbContext db)
        {
            db.Criptomoneda.RemoveRange(db.Criptomoneda);
            db.Red.RemoveRange(db.Red);
            db.SaveChanges();
        }

        public static IList<Criptomoneda> GetCriptomonedas(int index, int numOfCriptomonedas)
        {
            Red red1 = GetRedes(0, 1).First();
            Red red2 = GetRedes(1, 1).First();
            Red red3 = GetRedes(2, 1).First();


            var allCriptomonedas = new List<Criptomoneda>
            {
                new Criptomoneda { ID = 1, Nombre = "BNB", Precio = 445, PorcentajeVariacion = (float)-0.62, Capitalizacion = 74000000, Red= red1, CantidadAComprar=50, NombreRed = 2, CantidadAVender=30 },
                new Criptomoneda { ID = 2, Nombre = "Bitcoin", Precio = 52353, PorcentajeVariacion = (float)-1, Capitalizacion = 988000000, Red= red2, CantidadAComprar=1, NombreRed = 3, CantidadAVender=1 },
                new Criptomoneda { ID = 3, Nombre = "Ethereum", Precio = 3656, PorcentajeVariacion = (float)1.05, Capitalizacion = 434000000, Red= red3, CantidadAComprar=25, NombreRed = 1, CantidadAVender=22 },
                new Criptomoneda { ID = 7, Nombre = "AXS", Precio = 2, PorcentajeVariacion = (float)5, Capitalizacion = 56000000, Red= red1, CantidadAComprar=15000, NombreRed = 1, CantidadAVender=1000 }



            };

            return allCriptomonedas.GetRange(index, numOfCriptomonedas);
        }

        public static IList<Red> GetRedes(int index, int numOfRedes)
        {
            var allRedes = new List<Red>
                {
                    new Red { RedID=1, nombre = "Red Ethereum" },
                    new Red { RedID=2, nombre = "Red Binance" },
                    new Red { RedID=3, nombre = "Red Bitcoin" }

                };
            //return from the list as much instances as specified in numOfGenres
            return allRedes.GetRange(index, numOfRedes);
        }

    }
}

