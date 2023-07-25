using Exchange.Controllers;
using Exchange.Data;
using Exchange.Models;
using Exchange.Models.CriptomonedaViewModels;
using Exchange.UT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Exchange.UT.CriptomonedasController_test
{
    public class SeleccionCriptomonedas_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;
        Microsoft.AspNetCore.Http.DefaultHttpContext selectContext;

        public SeleccionCriptomonedas_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            
            UtilitiesForCriptomonedas.InitializeDbCriptomonedasForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            purchaseContext.User = identity;
            selectContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            selectContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSeleccionCriptomonedasParaCompra_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,4), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", null, 0, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(2,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, "Red Bitcoin", 0, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 445, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0, -0.62},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", "Red Ethereum", 445, -0.62},
            };

            return allTests;
        }
        public static IEnumerable<object[]> TestCasesForSelectCriptomonedasParaVender_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,4), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", null, 0, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(1,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, "Red Binance", 0, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 74000000, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0, -0.62},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", "Red Ethereum", 74000000, -0.62}

            };

            return allTests;
        }

        public static IEnumerable<object[]> TestCasesForSelectSelectCriptomonedasParaAlerta_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,4), UtilitiesForCriptomonedas.GetRedes(0,3), null, null,0 ,0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", null,0 ,0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(1,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, "Red Binance", 0 ,0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 74000000, 0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0 ,-0.62 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", "Ethereum", 74000000, -0.62 }
            };

            return allTests;
        }

        public static IEnumerable<object[]> TestCasesForSelectCriptomonedaForPrestamo_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,4), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0, 0, 0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", null, 0, 0, 0},
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(1,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, "Red Binance", 0, 0, 0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 445, 0, 0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0, 74000000, 0 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), null, null, 0, 0, -0.62 },
                new object[] { UtilitiesForCriptomonedas.GetCriptomonedas(0,1), UtilitiesForCriptomonedas.GetRedes(0,3), "BNB", "Red Ethereum", 445, 74000000, -0.62 }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSeleccionCriptomonedasParaCompra_get))]
        public async Task SeleccionCriptomonedasParaCompra_Get(List<Criptomoneda> expectedCriptomonedas, List<Red> expectedRedes, string filterNombre, string filterRed, int filterPre, float filterPor)
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                var expectedRedesNames = expectedRedes.Select(g => new { nameofRed = g.nombre });
                

                // Act
                var result = controller.SeleccionCriptomonedasParaCompra(filterNombre, filterRed, filterPre, filterPor);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SeleccionCriptomonedasParaCompraViewModel model = viewResult.Model as SeleccionCriptomonedasParaCompraViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);
                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Red.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRedesNames.SequenceEqual(modelRedes));
            }
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectCriptomonedasParaVender_get))]
        public async Task SelectCriptomonedasParaVender_Get(List<Criptomoneda> expectedCriptomonedas, List<Red> expectedRedes, string filterNombre, string filterRed, int filterCap, float filterPer)
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = selectContext;

                var expectedRedesNames = expectedRedes.Select(g => new { nameofRed = g.nombre });


                // Act
                var result = controller.SeleccionMonedasParaVender(filterNombre, filterRed, filterCap, filterPer);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                CriptomonedasParaVenderViewModel model = viewResult.Model as CriptomonedasParaVenderViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);
                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Redes.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRedesNames.SequenceEqual(modelRedes));
            }
        }


        [Theory]
        [MemberData(nameof(TestCasesForSelectSelectCriptomonedasParaAlerta_get))]
        public async Task SeleccionCriptomonedasParaAlerta_Get(List<Criptomoneda> expectedCriptomonedas, List<Red> expectedRedes, string filterNombre, string filterRed, int filterPre, float filterPor)
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                var expectedRedesNames = expectedRedes.Select(g => new { nameofRed = g.nombre });

                // Act
                var result = controller.SeleccionCriptomonedasParaAlerta(filterNombre, filterRed, filterPre, filterPor);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SeleccionCriptomonedasParaAlertaViewModel model = viewResult.Model as SeleccionCriptomonedasParaAlertaViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);
                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Red.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRedesNames.SequenceEqual(modelRedes));
            }
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectCriptomonedaForPrestamo_get))]
        public async Task SelectCriptomonedaForPrestamo_Get(List<Criptomoneda> expectedCriptomonedas, List<Red> expectedRedes, string filterNombre, string filterRed, int filterPrecio, int filterCap, float filterVar)
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = selectContext;

                var expectedRedesNames = expectedRedes.Select(g => new { nameofRed = g.nombre });

                // Act
                var result = controller.SelectCriptomonedaForPrestamo(filterNombre, filterRed, filterPrecio, filterCap, filterVar);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectCriptomonedasForPrestamoViewModel model = viewResult.Model as SelectCriptomonedasForPrestamoViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);
                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Redes.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRedesNames.SequenceEqual(modelRedes));
            }
        }


        [Fact]
        public void SeleccionCriptomonedasParaCompra_Post_CriptomonedasNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;
                //we create an array that is a list names of genres
                var expectedRed = UtilitiesForCriptomonedas.GetRedes(0, 3).Select(g => new { nameofRed = g.nombre });
                var expectedCriptomonedas = UtilitiesForCriptomonedas.GetCriptomonedas(0, 4);

                SeleccionadasCriptomonedasParaCompraViewModel selected = new SeleccionadasCriptomonedasParaCompraViewModel { IdsToAdd = null };

                // Act
                var result = controller.SeleccionCriptomonedasParaCompra(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SeleccionCriptomonedasParaCompraViewModel model = viewResult.Model as SeleccionCriptomonedasParaCompraViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);

                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Red.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRed.SequenceEqual(modelRedes));

            }
        }

        [Fact]
        public void SelectCriptomonedasParaVender_Post_CriptomonedasNoSeleccionadas()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = selectContext;
                //we create an array that is a list names of genres
                var expectedRedes = UtilitiesForCriptomonedas.GetRedes(0, 3).Select(g => new { nameofRed = g.nombre });
                var expectedCriptomonedas = UtilitiesForCriptomonedas.GetCriptomonedas(0, 4);

                MonedasSeleccionadasParaVenderViewModel selected = new MonedasSeleccionadasParaVenderViewModel { IdsToAdd = null };

                // Act
                var result = controller.SeleccionMonedasParaVender(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                CriptomonedasParaVenderViewModel model = viewResult.Model as CriptomonedasParaVenderViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);

                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Redes.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRedes.SequenceEqual(modelRedes));


            }
        }


        [Fact]
        public void SeleccionCriptomonedasParaAlerta_Post_CriptomonedasNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;
                //we create an array that is a list names of genres
                var expectedRed = UtilitiesForCriptomonedas.GetRedes(0, 3).Select(g => new { nameofRed = g.nombre });
                var expectedCriptomonedas = UtilitiesForCriptomonedas.GetCriptomonedas(0, 4);

                SeleccionadasCriptomonedasParaAlertaViewModel selected = new SeleccionadasCriptomonedasParaAlertaViewModel { IdsToAdd = null };

                // Act
                var result = controller.SeleccionCriptomonedasParaAlerta(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SeleccionCriptomonedasParaAlertaViewModel model = viewResult.Model as SeleccionCriptomonedasParaAlertaViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);

                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Red.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRed.SequenceEqual(modelRedes));

            }
        }




        [Fact]
        public void SeleccionCriptomonedasParaCompra_Post_CriptomonedasSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SeleccionadasCriptomonedasParaCompraViewModel criptomonedas = new SeleccionadasCriptomonedasParaCompraViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SeleccionCriptomonedasParaCompra(criptomonedas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentCriptomonedas = viewResult.RouteValues.Values.First();
                Assert.Equal(criptomonedas.IdsToAdd, currentCriptomonedas);

            }
        }

        [Fact]
        public void SelectCriptomonedasParaVender_Post_CriptomonedasSeleccionadas()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = selectContext;

                String[] ids = new string[1] { "1" };
                MonedasSeleccionadasParaVenderViewModel criptomonedas = new MonedasSeleccionadasParaVenderViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SeleccionMonedasParaVender(criptomonedas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentCriptomonedas = viewResult.RouteValues.Values.First();
                Assert.Equal(criptomonedas.IdsToAdd, currentCriptomonedas);

            }
        }

        [Fact]
        public void SelectCriptomonedasParaAlerta_Post_CriptomonedasSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SeleccionadasCriptomonedasParaAlertaViewModel Criptomonedas =
                    new SeleccionadasCriptomonedasParaAlertaViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SeleccionCriptomonedasParaAlerta(Criptomonedas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentCriptomonedas = viewResult.RouteValues.Values.First();
                Assert.Equal(Criptomonedas.IdsToAdd, currentCriptomonedas);

            }
        }

        [Fact]
        public void SelectCriptomonedaForPrestamo_Post_CriptomonedasNoSeleccionadas()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = selectContext;
                //we create an array that is a list names of genres
                var expectedRedes = UtilitiesForCriptomonedas.GetRedes(0, 3).Select(g => new { nameofRed = g.nombre });
                var expectedCriptomonedas = UtilitiesForCriptomonedas.GetCriptomonedas(0, 4);

                SelectedCriptomonedaForPrestamoViewModel selected = new SelectedCriptomonedaForPrestamoViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectCriptomonedaForPrestamo(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectCriptomonedasForPrestamoViewModel model = viewResult.Model as SelectCriptomonedasForPrestamoViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedCriptomonedas, model.Criptomonedas);

                //check that both collections (expected and result) have the same names of Genre
                var modelRedes = model.Redes.Select(g => new { nameofRed = g.Text });
                Assert.True(expectedRedes.SequenceEqual(modelRedes));

            }
        }

        [Fact]
        public void SelectCriptomonedaForPrestamo_Post_CriptomonedasSeleccionadas()
        {
            using (context)
            {

                // Arrange
                var controller = new CriptomonedasController(context);
                controller.ControllerContext.HttpContext = selectContext;

                String[] ids = new string[1] { "1" };
                SelectedCriptomonedaForPrestamoViewModel criptomonedas = new SelectedCriptomonedaForPrestamoViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectCriptomonedaForPrestamo(criptomonedas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentCriptomonedas = viewResult.RouteValues.Values.First();
                Assert.Equal(criptomonedas.IdsToAdd, currentCriptomonedas);

            }
        }
    }


}
