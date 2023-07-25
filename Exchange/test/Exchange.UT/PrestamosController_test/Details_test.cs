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


namespace Exchange.UT.PrestamosController_test
{
    public class Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext prestamoContext;

        public Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForPrestamos.InitializeDbPrestamosForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            prestamoContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            prestamoContext.User = identity;

        }


        public static IEnumerable<object[]> TestCasesFor_Prestamo_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Prestamo_notfound_withoutId))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Prestamo_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new PrestamosController(context);
                controller.ControllerContext.HttpContext = prestamoContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Prestamo_found()
        {
            // Arrange
            using (context)
            {
                var expectedPrestamo = UtilitiesForPrestamos.GetPrestamos(0, 1).First();
                var controller = new PrestamosController(context);
                controller.ControllerContext.HttpContext = prestamoContext;

                // Act
                var result = await controller.Details(expectedPrestamo.PrestamoID);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Prestamo;
                Assert.Equal(expectedPrestamo, model);

            }
        }
    }
}