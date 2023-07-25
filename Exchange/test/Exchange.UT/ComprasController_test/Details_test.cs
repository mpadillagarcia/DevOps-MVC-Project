using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Exchange.Controllers;
using Exchange.Data;
using Exchange.Models;


namespace Exchange.UT.ComprasController_test
{
    public class Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext compraContext;

        public Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForCompras.InitializeDbComprasForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            compraContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            compraContext.User = identity;

        }


        public static IEnumerable<object[]> TestCasesFor_Compra_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Compra_notfound_withoutId))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Compra_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = compraContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Compra_found()
        {
            // Arrange
            using (context)
            {
                var expectedCompra = UtilitiesForCompras.GetCompras(0, 1).First();
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = compraContext;

                // Act
                var result = await controller.Details(expectedCompra.CompraId);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Compra;
                Assert.Equal(expectedCompra, model);

            }
        }
    }
}