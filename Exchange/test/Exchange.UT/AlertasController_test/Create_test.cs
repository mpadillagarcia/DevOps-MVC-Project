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
using Exchange.Models.AlertaViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;
using Exchange.Models.CriptomonedaViewModels;
using Exchange.UT.CriptomonedasController_test;
using Exchange.UT.AlertasController_test;

namespace Exchange.UT.AlertasController_test
{
    public class Create_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext alertaContext;


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
            alertaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
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
                var controller = new AlertasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = alertaContext;

                String[] ids = new string[1] { "1" };
                SeleccionadasCriptomonedasParaAlertaViewModel criptomonedas = new() { IdsToAdd = ids };
                Criptomoneda expectedCriptomoneda = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First();
                Cliente expectedCliente = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<AlertaItemViewModel> expectedMonedaAlertar = new AlertaItemViewModel[1] {
                    new AlertaItemViewModel {ID = expectedCriptomoneda.ID, Nombre = expectedCriptomoneda.Nombre,
                        Precio = expectedCriptomoneda.Precio, NombreRed = expectedCriptomoneda.Red.nombre} };
                AlertaCreateViewModel expectedAlerta = new() { MonedaAlertar = expectedMonedaAlertar, Nombre = expectedCliente.Nombre, PrimerApellido = expectedCliente.PrimerApellido, SegundoApellido = expectedCliente.SegundoApellido };

                // Act
                var result = controller.Create(criptomonedas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                AlertaCreateViewModel currentAlerta = viewResult.Model as AlertaCreateViewModel;

                Assert.Equal(currentAlerta, expectedAlerta);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutCriptomoneda()
        {
            using (context)
            {

                // Arrange
                var controller = new AlertasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = alertaContext;
                Cliente cliente = Utilities.GetUsers(0, 1).First() as Cliente;
                SeleccionadasCriptomonedasParaAlertaViewModel criptomonedas = new();

                AlertaCreateViewModel expectedAlerta = new()
                {
                    Nombre = cliente.Nombre,
                    PrimerApellido = cliente.PrimerApellido,
                    SegundoApellido = cliente.SegundoApellido,
                    MonedaAlertar = new List<AlertaItemViewModel>()
                };


                // Act
                var result = controller.Create(criptomonedas);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                AlertaCreateViewModel currentAlerta = viewResult.Model as AlertaCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentAlerta, expectedAlerta);
                Assert.Equal("Por favor, debes de seleccionar al menos una criptomoneda para crear la alerta", error.ErrorMessage);
            }
        }

        

        public static IEnumerable<object[]> TestCasesForAlertasCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Alerta expectedAlerta1 = UtilitiesForAlertas.GetAlertas(0, 1).First();
            Cliente expectedCliente1 = expectedAlerta1.Cliente;
            //var expectedPayment1 = expectedPurchase1.PaymentMethod as CreditCard;
            MonedaAlerta expectedMonedaAlerta1 = expectedAlerta1.MonedaAlertar.First();
            int expectedPrecioAlertaForAlerta1 = UtilitiesForCriptomonedas.GetCriptomonedas(0, 1).First().CantidadAComprar - expectedMonedaAlerta1.PrecioAlerta;
            IList<AlertaItemViewModel> monedaAlertarViewModel1 = new AlertaItemViewModel[1] { new AlertaItemViewModel {
                    PrecioAlerta = expectedMonedaAlerta1.PrecioAlerta, ID = expectedMonedaAlerta1.ID,
                    Nombre=expectedMonedaAlerta1.Criptomoneda.Nombre, NombreRed=expectedMonedaAlerta1.Criptomoneda.Red.nombre,
                    Precio=expectedMonedaAlerta1.Criptomoneda.Precio} };
            AlertaCreateViewModel alerta1 = new()
            {
                Nombre = expectedCliente1.Nombre,
                PrimerApellido = expectedCliente1.PrimerApellido,
                SegundoApellido = expectedCliente1.SegundoApellido,
                MonedaAlertar = monedaAlertarViewModel1,
                FechaAlerta = expectedAlerta1.FechaAlerta,
                FechaExpira = expectedAlerta1.FechaExpira
            };


            /*
            Purchase expectedPurchase2 = UtilitiesForPurchases.GetPurchases(1, 1).First();
            expectedPurchase2.PurchaseId = 1;
            expectedPurchase2.PurchaseItems.First().Id = 1;
            expectedPurchase2.PurchaseItems.First().PurchaseId = 1;
            PurchaseItem expectedPurchaseItem2 = expectedPurchase2.PurchaseItems.First();
            int expectedQuantityForPurchase2 = UtilitiesForMovies.GetMovies(1, 1).First().QuantityForPurchase - expectedPurchaseItem2.Quantity;
            var expectedPayment2 = expectedPurchase2.PaymentMethod as PayPal;
            expectedPayment2.ID = 1;
            Customer expectedCustomer2 = expectedPurchase2.Customer;

            IList<PurchaseItemViewModel> purchaseItemsViewModel2 = new PurchaseItemViewModel[1] { new PurchaseItemViewModel {
                    Quantity = expectedPurchaseItem2.Quantity, MovieID = expectedPurchaseItem2.MovieId,
                    Title=expectedPurchaseItem2.Movie.Title, Genre=expectedPurchaseItem2.Movie.Genre.Name,
                    PriceForPurchase=expectedPurchaseItem2.Movie.PriceForPurchase} };
            PurchaseCreateViewModel purchase2 = new()
            {
                Name = expectedCustomer2.Name,
                FirstSurname = expectedCustomer2.FirstSurname,
                SecondSurname = expectedCustomer2.SecondSurname,
                PurchaseItems = purchaseItemsViewModel2,
                DeliveryAddress = expectedPurchase2.DeliveryAddress,
                PaymentMethod = "PayPal",
                Phone = expectedPayment2.Phone,
                Prefix = expectedPayment2.Prefix,
                Email = expectedPayment2.Email
            };
            */

            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { alerta1,  expectedAlerta1, expectedPrecioAlertaForAlerta1}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForAlertasCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(AlertaCreateViewModel alerta, Alerta expectedAlerta, int expectedPrecioAlertaForAlerta)
        {
            using (context)
            {

                // Arrange
                var controller = new AlertasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = alertaContext;

                // Act
                var result = controller.CreatePost(alerta);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualAlerta = context.Alerta.Include(p => p.MonedaAlertar).
                                    FirstOrDefault(p => p.Id == expectedAlerta.Id);
                Assert.Equal(expectedAlerta, actualAlerta);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                //Assert.Equal(expectedPrecioAlertaForAlerta,
                //context.Criptomoneda.First(m => m.ID == expectedAlerta.MonedaAlertar.First().ID).CantidadAComprar);


            }

        }
    }
}
