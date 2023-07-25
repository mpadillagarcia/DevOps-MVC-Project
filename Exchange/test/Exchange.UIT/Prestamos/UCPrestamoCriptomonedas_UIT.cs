using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
//Necesario para obtener Find dentro de las ICollection o IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;


namespace Exchange.UIT.Prestamos
{
    public class UCPrestamoCriptomonedas_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;

        public UCPrestamoCriptomonedas_UIT()
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);
            Initial_step_opening_the_web_page();
            //it is needed to run the scripts for udpating the database 
        }

        void IDisposable.Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }

        private void Initial_step_opening_the_web_page()
        {
            _driver.Navigate()
                .GoToUrl(_URI);
        }

        private void Precondition_perform_login()
        {

            _driver.Navigate()
                    .GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("peter@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("OtherPass12$");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }


        private void First_step_accessing_prestamos()
        {
            _driver.FindElement(By.Id("PrestamoController")).Click();    //  Shared/_Layout

        }


        private void Second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("SelectCriptomonedaForPrestamo")).Click();    //  Views/Criptomonedas/SelectCriptomonedaForPrestamo
                                                                                    //Alternatively we may use a linked text, i.e., any text that is between “<A>” and “</A”> tag
                                                                                    //_driver.FindElement(By.LinkText("Create New")).Click();

        }

        private void Third_filter_criptomonedas_byNombre(string nombreFilter)
        {
            _driver.FindElement(By.Id("NombreMoneda")).SendKeys(nombreFilter);    //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();              //  Views/Criptomonedas/SelectCriptomonedaForPrestamo
        }

        private void Third_filter_criptomonedas_byPrecio(string nombreFilter)
        {
            _driver.FindElement(By.Id("Precio")).Clear();

            _driver.FindElement(By.Id("Precio")).SendKeys(nombreFilter);    //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();              //  Views/Criptomonedas/SelectCriptomonedaForPrestamo
        }

        private void Third_filter_criptomonedas_byPorcentajeVariacion(string nombreFilter)
        {
            _driver.FindElement(By.Id("PorcentajeVariacion")).SendKeys(nombreFilter);    //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();              //  Views/Criptomonedas/SelectCriptomonedaForPrestamo
        }

        private void Third_filter_criptomonedas_byCapitalizacion(string nombreFilter)
        {
            _driver.FindElement(By.Id("Capitalizacion")).Clear();

            _driver.FindElement(By.Id("Capitalizacion")).SendKeys(nombreFilter);    //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();              //  Views/Criptomonedas/SelectCriptomonedaForPrestamo
        }

        private void Third_filter_criptomonedas_byRed(string redSelected)
        {

            var red = _driver.FindElement(By.Id("RedMonedaSeleccionada"));      //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

            //create select element object 
            SelectElement selectElement = new SelectElement(red);
            //select Action from the dropdown menu
            selectElement.SelectByText(redSelected);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();              //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

        }
        private void Third_select_criptomonedas_and_submit()
        {

            _driver.FindElement(By.Id("Criptomoneda_2")).Click();                 //  Views/Criptomonedas/SelectCriptomonedaForPrestamo               
            _driver.FindElement(By.Id("Criptomoneda_4")).Click();                 //  Views/Criptomonedas/SelectCriptomonedaForPrestamo
            _driver.FindElement(By.Id("nextButton")).Click();                     //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

        }

        private void Third_alternate_not_selecting_criptomonedas()
        {

            _driver.FindElement(By.Id("nextButton")).Click();                     //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

        }
        private void Fourth_fill_in_information_and_press_create(string quantityCriptomoneda1,
            string quantityCriptomoneda2, string MetodoPago, string NumeroTarjeta, string CVV, string FechaCaducidad,
            string Email, string Prefijo, string Tlf)
        {

            _driver.FindElement(By.Id("Criptomoneda_Cantidad_2")).Clear();                            // Views/Prestamos/Create
            _driver.FindElement(By.Id("Criptomoneda_Cantidad_2")).SendKeys(quantityCriptomoneda1);    // Views/Prestamos/Create

            _driver.FindElement(By.Id("Criptomoneda_Cantidad_4")).Clear();                            // Views/Prestamos/Create
            _driver.FindElement(By.Id("Criptomoneda_Cantidad_4")).SendKeys(quantityCriptomoneda2);    // Views/Prestamos/Create

            if (MetodoPago.Equals("TarjetaCredito"))
            {
                _driver.FindElement(By.Id("r11")).Click();

                _driver.FindElement(By.Id("NumeroTarjeta")).SendKeys(NumeroTarjeta);

                _driver.FindElement(By.Id("CVV")).SendKeys(CVV);

                _driver.FindElement(By.Id("FechaCaducidad")).Clear();
                _driver.FindElement(By.Id("FechaCaducidad")).SendKeys(FechaCaducidad);
            }
            else
            {
                _driver.FindElement(By.Id("r12")).Click();

                _driver.FindElement(By.Id("Email")).SendKeys(Email);

                _driver.FindElement(By.Id("Prefijo")).SendKeys(Prefijo);

                _driver.FindElement(By.Id("Tlf")).SendKeys(Tlf);
            }
            _driver.FindElement(By.Id("CreateButton")).Click();
        }

        [Theory]
        [ClassData(typeof(PrestamoCriptomonedasTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_basic_flow(string quantityCriptomoneda1,
            string quantityCriptomoneda2, string MetodoPago, string NumeroTarjeta, string CVV, string FechaCaducidad,
            string Email, string Prefijo, string Tlf)
        {
            //Arrange
            string[] expectedText = { "Details - Exchange","Details",
                "Prestamo","Peter","Jackson","FechaPrestamo",
                "TasaInteres","Cliente","Bitcoin","BUSD"};
            //Act
            Precondition_perform_login();
            First_step_accessing_prestamos();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(
                quantityCriptomoneda1, quantityCriptomoneda2, MetodoPago, NumeroTarjeta, CVV, FechaCaducidad, Email, Prefijo, Tlf);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        /*
	    [Fact(Skip = "As precondition, first execute script dbo.Movie.Quantity0 to update quantityforpurchase to 0")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_alternate_flow_1_NoMoviesAvailable()
        {
            //Arrange
            string expectedText = "There are no movies available";

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();

            var movieRow = _driver.FindElement(By.Id("NoMovies"));

            //checks the expected row exists
            Assert.NotNull(movieRow);
            Assert.Equal(expectedText, movieRow.Text);
        }
	    */


        [Theory]
        [InlineData("BNB", "445", "Red Binance", "-0.62", "74000000", "Nombre")]
        [InlineData("AXS", "2", "Red Ethereum", "5", "56000000", "Precio")] 
        [InlineData("BUSD", "1", "Red Binance", "-0.05", "11500000", "Red")]
        [InlineData("BUSD", "1", "Red Binance", "-0.05", "11500000", "PorcentajeVariacion")]
        [InlineData("BUSD", "1", "Red Binance", "-0.05", "11500000", "Capitalizacion")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4_alternate_flow_2_filteringbyNombre(string nombre, string precio,      
            string red, string porcentajeVariacion, string capitalizacion, string filter)
        {
            //Arrange
            string[] expectedText = { nombre, precio, red };

            //Act
            Precondition_perform_login();
            First_step_accessing_prestamos();
            Second_step_accessing_link_Create_New();
            if (filter.Equals("Nombre"))                                                    //  Views/Criptomonedas/Create
                Third_filter_criptomonedas_byNombre(nombre);
            else if (filter.Equals("Precio"))                                               //  Views/Criptomonedas/Create
                Third_filter_criptomonedas_byPrecio(precio);
            else if (filter.Equals("PorcentajeVariacion"))                                  //  Views/Criptomonedas/Create
                Third_filter_criptomonedas_byPorcentajeVariacion(porcentajeVariacion);
            else if (filter.Equals("Capitalizacion"))                                       //  Views/Criptomonedas/Create
                Third_filter_criptomonedas_byCapitalizacion(capitalizacion);
            else
                Third_filter_criptomonedas_byRed(red);

            var criptomonedaRow = _driver.FindElements(By.Id("Criptomoneda_Nombre_" + nombre));    //  Views/Criptomonedas/SelectCriptomonedaForPrestamo

            //checks the expected row exists
            Assert.NotNull(criptomonedaRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(criptomonedaRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_5_alternate_flow_3_criptomonedasNotSelected()
        {
            //Arrange
            string expectedText = "Debes seleccionar al menos una Criptomoneda";

            //Act
            Precondition_perform_login();
            First_step_accessing_prestamos();
            Second_step_accessing_link_Create_New();
            Third_alternate_not_selecting_criptomonedas();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        //[InlineData("2", "2", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "Please, set your address for delivery")]
        [InlineData("2", "2", "TarjetaCredito", "", "123", "12/12/2022", null, null, null, "Please, fill in your Credit Card Number for your Credit Card payment")]
        [InlineData("2", "2", "TarjetaCredito", "1234567890123456", "", "12/12/2022", null, null, null, "Please, fill in your CCV for your Credit Card payment")]
        [InlineData("2", "2", "TarjetaCredito", "1234567890123456", "123", "", null, null, null, "Please, fill in your ExpirationDate for your Credit Card payment")]
        [InlineData("2", "2", "PayPal", null, null, null, "", "967", "673240", "Please, fill in your Email for your PayPal payment")]
        [InlineData("2", "2", "PayPal", null, null, null, "peter@uclm.com", "", "673240", "Please, fill in your Prefix for your PayPal payment")]
        [InlineData("2", "2", "PayPal", null, null, null, "peter@uclm.com", "967", "", "Please, fill in your Phone for your PayPal payment")]
        [InlineData("", "2", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "The Quantity field is required")]
        //[InlineData("Calle de la Universidad 1, Albacete, 02006, España", "400", "2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "There are no enough movies titled Star Star Wars: The Force Awakens")]
        [InlineData("0", "0", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "Please, select Quantity higher than 0 for at least one criptomoneda")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6_UC1_6_15_alternate_flow_4_testingErrorsMandatorydata(string quantityCriptomoneda1, string quantityCriptomoneda2,
            string MetodoPago, string NumeroTarjeta, string CVV, string FechaCaducidad,
            string Email, string Prefijo, string Tlf, string expectedText)
        {

            //Act
            Precondition_perform_login();
            First_step_accessing_prestamos();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(
                quantityCriptomoneda1, quantityCriptomoneda2, MetodoPago, NumeroTarjeta, CVV, FechaCaducidad, Email, Prefijo, Tlf);


            //Assert
            //the expected error is shown in the view
            Assert.Contains(expectedText, _driver.PageSource);


        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_16_not_logged_in()
        {
            //Arrange
            string expectedText = "Use a local account to log in.";

            //Act
            First_step_accessing_prestamos();
            Second_step_accessing_link_Create_New();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }
    }
}
