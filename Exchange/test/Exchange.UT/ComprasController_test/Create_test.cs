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
using Exchange.Models.CompraViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;
using Exchange.Models.CriptomonedaViewModels;
using Exchange.UT.CriptomonedasController_test;
using Exchange.UT.ComprasController_test;

namespace Exchange.UT.ComprasController_test
{
    public class Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;


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
            compraContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
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
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                String[] ids = new string[1] { "1" };
                SeleccionadasCriptomonedasParaCompraViewModel criptomonedas = new() { IdsToAdd = ids };
                Criptomoneda expectedCriptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
                Cliente expectedCliente = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<CompraItemViewModel> expectedCompraItems = new CompraItemViewModel[1] {
                    new CompraItemViewModel {Cantidad=0, ID = expectedCriptomoneda.ID, Nombre = expectedCriptomoneda.Nombre,
                        Precio = expectedCriptomoneda.Precio, Red = expectedCriptomoneda.Red.nombre} };
                CompraCreateViewModel expectedCompra = new() { CompraItems = expectedCompraItems, Nombre = expectedCliente.Nombre, PrimerApellido = expectedCliente.PrimerApellido, SegundoApellido = expectedCliente.SegundoApellido };

                // Act
                var result = controller.Create(criptomonedas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;

                Assert.Equal(currentCompra, expectedCompra);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutCriptomoneda()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;
                Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
                SeleccionadasCriptomonedasParaCompraViewModel criptomonedas = new();

                CompraCreateViewModel expectedCompra = new()
                {
                    Nombre = cliente.Nombre,
                    PrimerApellido = cliente.PrimerApellido,
                    SegundoApellido = cliente.SegundoApellido,
                    CompraItems = new List<CompraItemViewModel>()
                };


                // Act
                var result = controller.Create(criptomonedas);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentCompra, expectedCompra);
                Assert.Equal("Debes seleccionar al menos una criptomoneda", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForComprasCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughCriptomonedasToBeComprad

            Criptomoneda criptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            //  var payment1 = new PayPal { Email = cliente.Email, Tlf = cliente.TlfNumber, Prefijo = "+34" };

            //Input values
            //IList<CompraItemViewModel> compraItemsViewModel1 = new CompraItemViewModel[1] { new CompraItemViewModel { Cantidad = 10, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            //CompraCreateViewModel compra1 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, CompraItems = compraItemsViewModel1, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };

            //Expected values
            //IList<CompraItemViewModel> expectedCompraItemsViewModel1 = new CompraItemViewModel[1] { new CompraItemViewModel { Cantidad = 10, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            //CompraCreateViewModel expectedCompraVM1 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, CompraItems = expectedCompraItemsViewModel1, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };
            //string expetedErrorMessage1 = "There are no enough criptomonedas titled The lord of the rings, please select less or equal than 5";


            //Second error: Create_Post_WithCantidad0ForCompra

            ////Input values
            IList<CompraItemViewModel> compraItemsViewModel2 = new CompraItemViewModel[1] { new CompraItemViewModel { Cantidad = 10000000, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            CompraCreateViewModel compra2 = new() { Nombre = cliente.Nombre, MetodoPago = "Paypal", PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, CompraItems = compraItemsViewModel2, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };

            ////expected values
            IList<CompraItemViewModel> expectedCompraItemsViewModel2 = new CompraItemViewModel[1] { new CompraItemViewModel { Cantidad = 10000000, ID = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            CompraCreateViewModel expectedCompraVM2 = new() { Nombre = cliente.Nombre, MetodoPago = "Paypal", PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, CompraItems = expectedCompraItemsViewModel2, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };
            string expetedErrorMessage2 = $"No hay suficientes Criptomonedas con el nombre { criptomoneda.Nombre}, porfavor selecciona menos o igual a { criptomoneda.CantidadAComprar}";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                //new object[] { compra1,  expectedCompraVM1, expetedErrorMessage1 }
                //,
                new object[] { compra2,  expectedCompraVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForComprasCreatePost_WithErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithErrors(CompraCreateViewModel compra, CompraCreateViewModel expectedCompraVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.CreatePost(compra);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CompraCreateViewModel currentCompra = viewResult.Model as CompraCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedCompraVM, currentCompra);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForComprasCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Compra with TarjetaCredito
            Compra expectedCompra1 = UtilitiesForCompras.GetCompras(0, 1).First();
            Cliente expectedCliente1 = expectedCompra1.Cliente;
            var expectedPayment1 = expectedCompra1.MetodoPago as TarjetaCredito;
            CompraItem expectedCompraItem1 = expectedCompra1.CompraItems.First();
            int expectedCantidadForCompra1 = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First().CantidadAComprar - expectedCompraItem1.Cantidad;
            IList<CompraItemViewModel> compraItemsViewModel1 = new CompraItemViewModel[1] { new CompraItemViewModel {
                    Cantidad = expectedCompraItem1.Cantidad, ID = expectedCompraItem1.CriptomonedaId,
                    Nombre=expectedCompraItem1.Criptomoneda.Nombre, Red=expectedCompraItem1.Criptomoneda.Red.nombre,
                    Precio=expectedCompraItem1.Criptomoneda.Precio} };
            CompraCreateViewModel compra1 = new()
            {
                Nombre = expectedCliente1.Nombre,
                PrimerApellido = expectedCliente1.PrimerApellido,
                SegundoApellido = expectedCliente1.SegundoApellido,
                CompraItems = compraItemsViewModel1,
                MetodoPago = "TarjetaCredito",
                NumeroTarjeta = expectedPayment1.NumeroTarjeta,
                CVV = expectedPayment1.CVV,
                FechaExpiracion = expectedPayment1.FechaCaducidad

            };

            //Payment with Paypal
            Compra expectedCompra2 = UtilitiesForCompras.GetCompras(1, 1).First();
            expectedCompra2.CompraId = 1;
            expectedCompra2.CompraItems.First().Id = 1;
            expectedCompra2.CompraItems.First().CompraId = 1;
            CompraItem expectedCompraItem2 = expectedCompra2.CompraItems.First();
            int expectedCantidadForCompra2 = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First().CantidadAComprar - expectedCompraItem2.Cantidad;
            var expectedPayment2 = expectedCompra2.MetodoPago as PayPal;
            expectedPayment2.ID = 1;
            Cliente expectedCliente2 = expectedCompra2.Cliente;

            IList<CompraItemViewModel> compraItemsViewModel2 = new CompraItemViewModel[1] { new CompraItemViewModel {
                    Cantidad = expectedCompraItem2.Cantidad, ID = expectedCompraItem2.CriptomonedaId,
                    Nombre=expectedCompraItem2.Criptomoneda.Nombre, Red=expectedCompraItem2.Criptomoneda.Red.nombre,
                    Precio=expectedCompraItem2.Criptomoneda.Precio} };
            CompraCreateViewModel compra2 = new()
            {
                Nombre = expectedCliente2.Nombre,
                PrimerApellido = expectedCliente2.PrimerApellido,
                SegundoApellido = expectedCliente2.SegundoApellido,
                CompraItems = compraItemsViewModel2,
                MetodoPago = "PayPal",
                Tlf = expectedPayment2.Tlf,
                Prefijo = expectedPayment2.Prefijo,
                Email = expectedPayment2.Email
            };

            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { compra1,  expectedCompra1, expectedCantidadForCompra1},
                new object[] { compra2,  expectedCompra2, expectedCantidadForCompra2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForComprasCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(CompraCreateViewModel compra, Compra expectedCompra, int expectedCantidadForCompra)
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = controller.CreatePost(compra);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the compra has been created in the database
                var actualCompra = context.Compra.Include(p => p.CompraItems).
                                    FirstOrDefault(p => p.CompraId == expectedCompra.CompraId);
                Assert.Equal(expectedCompra, actualCompra);

                //And that the quantity for compra of each associated criptomoneda has been modified accordingly 
                Assert.Equal(expectedCantidadForCompra,
                    context.Criptomoneda.First(m => m.ID == expectedCompra.CompraItems.First().CriptomonedaId).CantidadAComprar);


            }

        }



    }
}

