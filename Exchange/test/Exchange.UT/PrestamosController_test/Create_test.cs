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
using Exchange.Models.PrestamoViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;
using Exchange.Models.CriptomonedaViewModels;
using Exchange.UT.CriptomonedasController_test;
using Exchange.UT.PrestamosController_test;

namespace Exchange.UT.PrestamosController_test
{
    public class Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext prestamoContext;


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
            prestamoContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
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
                var controller = new PrestamosController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = prestamoContext;

                String[] ids = new string[1] { "1" };
                SelectedCriptomonedaForPrestamoViewModel criptomonedas = new() { IdsToAdd = ids };
                Criptomoneda expectedCriptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
                Cliente expectedCliente = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<PrestamoItemViewModel> expectedPrestamoItems = new PrestamoItemViewModel[1] {
                    new PrestamoItemViewModel {Cantidad=0, CriptomonedaId = expectedCriptomoneda.ID, Nombre = expectedCriptomoneda.Nombre, Red = expectedCriptomoneda.Red.nombre,
                        Precio = expectedCriptomoneda.Precio, PorcentajeVariacion = expectedCriptomoneda.PorcentajeVariacion, Capitalizacion = expectedCriptomoneda.Capitalizacion} };
                PrestamoCreateViewModel expectedPrestamo = new() { PrestamoItems = expectedPrestamoItems, Nombre = expectedCliente.Nombre, PrimerApellido = expectedCliente.PrimerApellido, SegundoApellido = expectedCliente.SegundoApellido };

                // Act
                var result = controller.Create(criptomonedas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                PrestamoCreateViewModel currentPrestamo = viewResult.Model as PrestamoCreateViewModel;

                Assert.Equal(currentPrestamo, expectedPrestamo);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutCriptomoneda()
        {
            using (context)
            {

                // Arrange
                var controller = new PrestamosController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = prestamoContext;
                Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedCriptomonedaForPrestamoViewModel criptomonedas = new();

                PrestamoCreateViewModel expectedPrestamo = new()
                {
                    Nombre = cliente.Nombre,
                    PrimerApellido = cliente.PrimerApellido,
                    SegundoApellido = cliente.SegundoApellido,
                    PrestamoItems = new List<PrestamoItemViewModel>()
                };


                // Act
                var result = controller.Create(criptomonedas);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                PrestamoCreateViewModel currentPrestamo = viewResult.Model as PrestamoCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentPrestamo, expectedPrestamo);
                Assert.Equal("You should select at least a Criptomoneda to be lent, please", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForPrestamosCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.

            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Criptomoneda criptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
            Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
            //var payment1 = new PayPal { Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //Input values
            //IList<PrestamoItemViewModel> prestamoItemsViewModel1 = new PrestamoItemViewModel[1] { new PrestamoItemViewModel { Cantidad = 10, CriptomonedaId = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            //PrestamoCreateViewModel prestamo1 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, PrestamoItems = prestamoItemsViewModel1, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };

            //Expected values
            //IList<PrestamoItemViewModel> expectedPrestamoItemsViewModel1 = new PrestamoItemViewModel[1] { new PrestamoItemViewModel { Cantidad = 10, CriptomonedaId = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            //PrestamoCreateViewModel expectedPrestamoVM1 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, PrestamoItems = expectedPrestamoItemsViewModel1, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };
            //string expetedErrorMessage1 = "There are no enough criptomonedas named BNB, please select less or equal than 5";


            //Second error: Create_Post_WithQuantity0ForPurchase

            ////Input values
            IList<PrestamoItemViewModel> prestamoItemsViewModel2 = new PrestamoItemViewModel[1] { new PrestamoItemViewModel { Cantidad = -99, CriptomonedaId = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            PrestamoCreateViewModel prestamo2 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, PrestamoItems = prestamoItemsViewModel2, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };

            ////expected values
            IList<PrestamoItemViewModel> expectedPrestamoItemsViewModel2 = new PrestamoItemViewModel[1] { new PrestamoItemViewModel { Cantidad = -99, CriptomonedaId = criptomoneda.ID, Nombre = criptomoneda.Nombre, Red = criptomoneda.Red.nombre, Precio = criptomoneda.Precio } };
            PrestamoCreateViewModel expectedPrestamoVM2 = new() { Nombre = cliente.Nombre, PrimerApellido = cliente.PrimerApellido, SegundoApellido = cliente.SegundoApellido, PrestamoItems = expectedPrestamoItemsViewModel2, Email = cliente.Email, Tlf = cliente.PhoneNumber, Prefijo = "+34" };
            string expetedErrorMessage2 = "Please select at least a Criptomoneda to be lent or cancel your prestamo";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                //new object[] { prestamo1,  expectedPrestamoVM1, expetedErrorMessage1 }
                //,
                new object[] { prestamo2,  expectedPrestamoVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPrestamosCreatePost_WithErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithErrors(PrestamoCreateViewModel prestamo, PrestamoCreateViewModel expectedPrestamoVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new PrestamosController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = prestamoContext;

                // Act
                var result = controller.CreatePost(prestamo);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                PrestamoCreateViewModel currentPrestamo = viewResult.Model as PrestamoCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedPrestamoVM, currentPrestamo);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForPrestamosCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Prestamo expectedPrestamo1 = UtilitiesForPrestamos.GetPrestamos(0, 1).First();
            Cliente expectedCliente1 = expectedPrestamo1.Cliente;
            var expectedPayment1 = expectedPrestamo1.MetodoPago as TarjetaCredito;
            MonedaPrestada expectedPrestamoItem1 = expectedPrestamo1.MonedasPrestadas.First();
            //int expectedQuantityForPurchase1 = UtilitiesForMovies.GetMovies(0, 1).First().QuantityForPurchase - expectedPurchaseItem1.Quantity;
            IList<PrestamoItemViewModel> prestamoItemsViewModel1 = new PrestamoItemViewModel[1] { new PrestamoItemViewModel {
                    Cantidad = expectedPrestamoItem1.Cantidad, CriptomonedaId = expectedPrestamoItem1.CriptomonedaId,
                    Nombre=expectedPrestamoItem1.Criptomoneda.Nombre, Red=expectedPrestamoItem1.Criptomoneda.Red.nombre,
                    Precio=expectedPrestamoItem1.Criptomoneda.Precio} };
            PrestamoCreateViewModel prestamo1 = new()
            {
                Nombre = expectedCliente1.Nombre,
                PrimerApellido = expectedCliente1.PrimerApellido,
                SegundoApellido = expectedCliente1.SegundoApellido,
                PrestamoItems = prestamoItemsViewModel1,
                MetodoPago = "CreditCard",
                NumeroTarjeta = expectedPayment1.NumeroTarjeta,
                CVV = expectedPayment1.CVV,
                FechaCaducidad = expectedPayment1.FechaCaducidad

            };

            //Payment with Paypal
            Prestamo expectedPrestamo2 = UtilitiesForPrestamos.GetPrestamos(1, 1).First();
            expectedPrestamo2.PrestamoID = 1;
            expectedPrestamo2.MonedasPrestadas.First().ID = 1;
            expectedPrestamo2.MonedasPrestadas.First().PrestamoId = 1;
            MonedaPrestada expectedPrestamoItem2 = expectedPrestamo2.MonedasPrestadas.First();
            //int expectedQuantityForPurchase2 = UtilitiesForMovies.GetMovies(1, 1).First().QuantityForPurchase - expectedPurchaseItem2.Quantity;
            var expectedPayment2 = expectedPrestamo2.MetodoPago as PayPal;
            expectedPayment2.ID = 1;
            Cliente expectedCliente2 = expectedPrestamo2.Cliente;

            IList<PrestamoItemViewModel> prestamoItemsViewModel2 = new PrestamoItemViewModel[1] { new PrestamoItemViewModel {
                    Cantidad = expectedPrestamoItem2.Cantidad, CriptomonedaId = expectedPrestamoItem2.CriptomonedaId,
                    Nombre=expectedPrestamoItem2.Criptomoneda.Nombre, Red=expectedPrestamoItem2.Criptomoneda.Red.nombre,
                    Precio=expectedPrestamoItem2.Criptomoneda.Precio} };
            PrestamoCreateViewModel prestamo2 = new()
            {
                Nombre = expectedCliente2.Nombre,
                PrimerApellido = expectedCliente2.PrimerApellido,
                SegundoApellido = expectedCliente2.SegundoApellido,
                PrestamoItems = prestamoItemsViewModel2,
                MetodoPago = "PayPal",
                Tlf = expectedPayment2.Tlf,
                Prefijo = expectedPayment2.Prefijo,
                Email = expectedPayment2.Email
            };

            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { prestamo1,  expectedPrestamo1 },
                new object[] { prestamo2, expectedPrestamo2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPrestamosCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(PrestamoCreateViewModel prestamo, Prestamo expectedPrestamo)
        {
            using (context)
            {

                // Arrange
                var controller = new PrestamosController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = prestamoContext;

                // Act
                var result = controller.CreatePost(prestamo);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the prestamo has been created in the database
                var actualPrestamo = context.Prestamo.Include(p => p.MonedasPrestadas).
                                    FirstOrDefault(p => p.PrestamoID == expectedPrestamo.PrestamoID);
                Assert.Equal(expectedPrestamo, actualPrestamo);

                //And that the quantity for prestamo of each associated criptomoneda has been modified accordingly 
                //Assert.Equal(expectedQuantityForPurchase,
                //context.Movie.First(m => m.MovieID == expectedPurchase.PurchaseItems.First().MovieId).QuantityForPurchase);


            }

        }
    }
}
