using Exchange.Controllers;
using Exchange.Data;
using Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Exchange.Models.VentaViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;
using Exchange.Models.CriptomonedaViewModels;
using Exchange.UT.CriptomonedasController_test;
using Exchange.UT.VentasController_test;

namespace Exchange.UT.VentasController_test
{
    public class Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext ventaContext;


        public Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForCriptomonedas.InitializeDbCriptomonedasForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            ventaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = identity
            };

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithSelectedCriptomonedas()
        {
            using (context)
            {

                // Arrange
                var controller = new VentasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = ventaContext;

                String[] ids = new string[1] { "1" };
                MonedasSeleccionadasParaVenderViewModel criptomonedas = new() { IdsToAdd = ids };
                Criptomoneda expectedmoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
                Cliente expectedCliente = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<VentaItemViewModel> expectedMonedasVendidas = new VentaItemViewModel[1] {
                    new VentaItemViewModel {CantidadAVender=0, ID = expectedmoneda.ID, Nombre = expectedmoneda.Nombre,
                        Precio = expectedmoneda.Precio, Red = expectedmoneda.Red.nombre} };
                VentaCreateViewModel expectedVenta = new() { MonedasVendidas = expectedMonedasVendidas, Nombre = expectedCliente.Nombre, PrimerApellido = expectedCliente.PrimerApellido, SegundoApellido = expectedCliente.SegundoApellido };

                // Act
                var result = controller.Create(criptomonedas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                VentaCreateViewModel currentVenta = viewResult.Model as VentaCreateViewModel;

                Assert.Equal(currentVenta, expectedVenta);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutCriptomoneda()
        {
            using (context)
            {

                // Arrange
                var controller = new VentasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = ventaContext;
                Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
                MonedasSeleccionadasParaVenderViewModel criptomonedas = new();

                VentaCreateViewModel expectedVenta = new()
                {
                    Nombre = cliente.Nombre,
                    PrimerApellido = cliente.PrimerApellido,
                    SegundoApellido = cliente.SegundoApellido,
                    MonedasVendidas = new List<VentaItemViewModel>()
                };


                // Act
                var result = controller.Create(criptomonedas);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                VentaCreateViewModel currentVenta = viewResult.Model as VentaCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentVenta, expectedVenta);
                Assert.Equal("Debes seleccionar al menos una moneda para vender", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForVentasCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Criptomoneda criptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            //  var payment1 = new PayPal { Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //Input values
            //IList<VentaItemViewModel> monedasVendidasViewModel1 = new VentaItemViewModel[1] { new VentaItemViewModel { CantidadAVender = 10, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            //VentaCreateViewModel venta1 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido,MetodoPago="Paypal", SegundoApellido = cliente.SegundoApellido, MonedasVendidas = monedasVendidasViewModel1, Email = cliente.Email, tlf = cliente.PhoneNumber, Prefijo = "+34" };

            //Expected values
            //IList<VentaItemViewModel> expectedMonedasVendidasViewModel1 = new VentaItemViewModel[1] { new VentaItemViewModel { CantidadAVender = 10, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            //VentaCreateViewModel expectedVentaVM1 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, MonedasVendidas = expectedMonedasVendidasViewModel1, Email = cliente.Email, tlf = cliente.PhoneNumber, Prefijo = "+34" };

            //string expectedErrorMessage1 = "There are no enough movies titled The lord of the rings, please select less or equal than 5";


            //Second error: Create_Post_WithQuantity0ForVenta

            //Input values
            IList<VentaItemViewModel> monedasVendidasViewModel2 = new VentaItemViewModel[1] { new VentaItemViewModel { CantidadAVender = 1000000, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            VentaCreateViewModel venta2 = new() { Nombre = cliente.Nombre, MetodoPago="PayPal", PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, MonedasVendidas = monedasVendidasViewModel2, Email = cliente.Email, tlf = cliente.PhoneNumber, Prefijo = "+34" };

            //expected values
            IList<VentaItemViewModel> expectedMonedasVendidasViewModel2 = new VentaItemViewModel[1] { new VentaItemViewModel { CantidadAVender = 1000000, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            VentaCreateViewModel expectedVentaVM2 = new() { Nombre = cliente.Nombre, MetodoPago = "PayPal", PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, MonedasVendidas = expectedMonedasVendidasViewModel2, Email = cliente.Email, tlf = cliente.PhoneNumber, Prefijo = " +34" };
            string expectedErrorMessage2 = $"No hay tanto {criptomoneda.Nombre}, selecciona menos o igual a {criptomoneda.CantidadAVender}";
           

            var allTests = new List<object[]>
                       {                  //Input values                                       // expected values
                //new object[] { venta1,  expectedVentaVM1, expectedErrorMessage1 }
                //,
                new object[] { venta2,  expectedVentaVM2, expectedErrorMessage2 }
                       };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForVentasCreatePost_WithErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithErrors(VentaCreateViewModel venta, VentaCreateViewModel expectedVentaVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new VentasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = ventaContext;

                // Act
                var result = controller.CreatePost(venta);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                VentaCreateViewModel currentVenta = viewResult.Model as VentaCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedVentaVM, currentVenta);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForVentasCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Venta con tarjeta
            Venta expectedVenta1 = UtilitiesForVentas.GetVentas(0, 1).First();
            Cliente expectedCliente1 = expectedVenta1.Cliente;
            var expectedPago1 = expectedVenta1.MetodoPago as TarjetaCredito;
            MonedaVendida expectedMonedaVendida1 = expectedVenta1.MonedasVendidas.First();
            int expectedQuantityForVenta1 = (int)(UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First().CantidadAVender - expectedMonedaVendida1.CantidadVenta);
            IList<VentaItemViewModel> monedasVendidasViewModel1 = new VentaItemViewModel[1] { new VentaItemViewModel {
                    CantidadAVender = (int)expectedMonedaVendida1.CantidadVenta, ID = expectedMonedaVendida1.Id,
                    Nombre=expectedMonedaVendida1.Criptomoneda.Nombre, Red=expectedMonedaVendida1.Criptomoneda.Red.nombre,
                    Precio=expectedMonedaVendida1.Criptomoneda.Precio} };
            VentaCreateViewModel venta1 = new()
            {
                Nombre = expectedCliente1.Nombre,
                PrimerApellido = expectedCliente1.PrimerApellido,
                SegundoApellido = expectedCliente1.SegundoApellido,
                MonedasVendidas = monedasVendidasViewModel1,
                MetodoPago = "TarjetaCredito",
                NumeroTarjeta = expectedPago1.NumeroTarjeta,
                CVV = expectedPago1.CVV,
                FechaCaducidad = expectedPago1.FechaCaducidad,
                

            };

            //Venta con Paypal
            Venta expectedVenta2 = UtilitiesForVentas.GetVentas(1, 1).First();
            expectedVenta2.VentaId = 1;
            expectedVenta2.MonedasVendidas.First().Id = 1;
            expectedVenta2.MonedasVendidas.First().VentaId = 1;
            MonedaVendida expectedMonedaVendida2 = expectedVenta2.MonedasVendidas.First();
            int expectedQuantityForVenta2 = (int)(UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First().CantidadAVender - expectedMonedaVendida2.CantidadVenta);
            var expectedPago2 = expectedVenta2.MetodoPago as PayPal;
            expectedPago2.ID = 1;
            Cliente expectedCliente2 = expectedVenta2.Cliente;

            IList<VentaItemViewModel> monedasVendidasViewModel2 = new VentaItemViewModel[1] { new VentaItemViewModel {
                    CantidadAVender = (int)expectedMonedaVendida2.CantidadVenta, ID = expectedMonedaVendida2.Id,
                    Nombre=expectedMonedaVendida2.Criptomoneda.Nombre, Red=expectedMonedaVendida2.Criptomoneda.Red.nombre,
                    Precio=expectedMonedaVendida2.Criptomoneda.Precio} };
            VentaCreateViewModel venta2 = new()
            {
                Nombre = expectedCliente2.Nombre,
                PrimerApellido = expectedCliente2.PrimerApellido,
                SegundoApellido = expectedCliente2.SegundoApellido,
                MonedasVendidas = monedasVendidasViewModel2,
                MetodoPago = "PayPal",
                tlf = expectedPago2.Tlf,
                Prefijo = expectedPago2.Prefijo,
                Email = expectedPago2.Email
            };

            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { venta1,  expectedVenta1, expectedQuantityForVenta1},
                new object[] { venta2,  expectedVenta2, expectedQuantityForVenta2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForVentasCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(VentaCreateViewModel venta, Venta expectedVenta, int expectedQuantityForVenta)
        {
            using (context)
            {

                // Arrange
                var controller = new VentasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = ventaContext;

                // Act
                var result = controller.CreatePost(venta);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualVenta = context.Venta.Include(p => p.MonedasVendidas).
                                    FirstOrDefault(p => p.VentaId == expectedVenta.VentaId);
               
                Assert.Equal(expectedVenta, actualVenta);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.Equal(expectedQuantityForVenta,
                    context.Criptomoneda.First(m => m.ID == expectedVenta.MonedasVendidas.First().Id).CantidadAVender);


            }

        }



    }
}