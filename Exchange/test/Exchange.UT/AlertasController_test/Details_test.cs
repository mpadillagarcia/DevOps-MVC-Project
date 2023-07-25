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

namespace Exchange.UT.AlertasController_test
{
    public class Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;

        public Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForAlertas.InitializeDbAlertasForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            purchaseContext.User = identity;

        }


        public static IEnumerable<object[]> TestCasesFor_Alerta_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Alerta_notfound_withoutId))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Alerta_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new AlertasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Alerta_found()
        {
            // Arrange
            using (context)
            {
                var expectedAlerta = UtilitiesForAlertas.GetAlertas(0, 1).First();
                var controller = new AlertasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                // Act
                var result = await controller.Details(expectedAlerta.Id);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Alerta;
                Assert.Equal(expectedAlerta, model);
            }
        }
    }
}
