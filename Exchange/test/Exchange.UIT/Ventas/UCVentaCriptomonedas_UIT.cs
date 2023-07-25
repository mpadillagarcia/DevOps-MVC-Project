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


namespace Exchange.UIT.Ventas
{
    public class UCVentaCriptomonedas_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;

        public UCVentaCriptomonedas_UIT()
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


        private void First_step_accessing_ventas()
        {
          
            _driver.FindElement(By.Id("VentaController")).Click();
        }


        private void Second_step_accessing_link_Create_New()
        {
            
            //Alternatively we may use a linked text, i.e., any text that is between “<A>” and “</A”> tag
            _driver.FindElement(By.LinkText("Create New")).Click();

        }

        private void Third_filter_criptomonedas_byNombre(string titleFilter)
        {
            _driver.FindElement(By.Id("NombreMoneda")).SendKeys(titleFilter);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();
        }

        private void Third_filter_criptomonedas_byCapitalizacion(string CapFilter)
        {
            _driver.FindElement(By.Id("Capitalizacion")).SendKeys(CapFilter);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();
        }

        private void Third_filter_criptomonedas_byPorcentaje(string PorcentajeFilter)
        {
            _driver.FindElement(By.Id("PorcentajeVariacion")).SendKeys(PorcentajeFilter);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();
        }

        private void Third_filter_criptomonedas_byRed(string redSelected)
        {

            var red = _driver.FindElement(By.Id("RedMonedaSeleccionada"));

            //create select element object 
            SelectElement selectElement = new SelectElement(red);
            //select Action from the dropdown menu
            selectElement.SelectByText(redSelected);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();

        }
        private void Third_select_criptomonedas_and_submit()
        {

            _driver.FindElement(By.Id("Criptomoneda_2")).Click();
            _driver.FindElement(By.Id("Criptomoneda_4")).Click();
            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void Third_alternate_not_selecting_criptomonedas()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }
        private void Fourth_fill_in_information_and_press_create(string cantidadMoneda1,
            string cantidadMoneda2, string MetodoPago, string NumeroTarjeta, string CVV, string FechaCaducidad,
            string Email, string Prefijo, string Tlf)
        {

            _driver.FindElement(By.Id("Criptomoneda_Cantidad_2")).Clear();
            _driver.FindElement(By.Id("Criptomoneda_Cantidad_2")).SendKeys(cantidadMoneda1);

            _driver.FindElement(By.Id("Criptomoneda_Cantidad_4")).Clear();
            _driver.FindElement(By.Id("Criptomoneda_Cantidad_4")).SendKeys(cantidadMoneda2);

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

                _driver.FindElement(By.Id("tlf")).SendKeys(Tlf);
            }
            _driver.FindElement(By.Id("CreateButton")).Click();
        }

        [Theory]
        [ClassData(typeof(VentaCriptomonedasTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_basic_flow(string cantidadMoneda1,
            string cantidadMoneda2, string MetodoPago, string NumeroTarjeta, string CVV, string FechaCaducidad,
            string Email, string Prefijo, string Tlf)
        {
            //Arrange
            string[] expectedText = { "Details - Exchange","Details",
                "Venta","FechaVenta","3","Peter","Jackson",
                "EquivEuros","Bitcoin","BUSD"};
            //Act
            Precondition_perform_login();
            First_step_accessing_ventas();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(cantidadMoneda1, cantidadMoneda2, MetodoPago, NumeroTarjeta, CVV, FechaCaducidad, Email, Prefijo, Tlf);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        //[Fact(Skip = "As precondition, first execute script dbo.Criptomoneda.Quantity0 to update CantidadAVender to 0")]
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_alternate_flow_1_NoCriptomonedasAvailable()
        {
            //Arrange
            string expectedText = "No hay criptomonedas disponibles";

            //Act
            Precondition_perform_login();
            First_step_accessing_ventas();
            Second_step_accessing_link_Create_New();

            var criptomonedaRow = _driver.FindElement(By.Id("NoCriptomonedas"));

            //checks the expected row exists
            Assert.NotNull(criptomonedaRow);
            Assert.Equal(expectedText, criptomonedaRow.Text);
        }


        [Theory]
        [InlineData("Bitcoin", "5","988000000","-1", "Red Bitcoin", "Nombre")]
        [InlineData("BUSD", "1","11500000","-0,05", "Red Binance", "Red")]
        [InlineData("Bitcoin", "5", "988000000", "-1", "Red Bitcoin", "Capitalizacion")]
        [InlineData("BUSD", "1", "11500000", "-0,05", "Red Binance", "Porcentaje")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4_alternate_flow_2_filteringbyNombre(string nombre, string precio, string capitalizacion, string porcentaje,
            string red, string filter)
        {
            //Arrange
            string[] expectedText = { nombre, precio, capitalizacion, porcentaje, red };

            //Act
            Precondition_perform_login();
            First_step_accessing_ventas();
            Second_step_accessing_link_Create_New();
            if (filter.Equals("Nombre"))
                Third_filter_criptomonedas_byNombre(nombre);
            else if (filter.Equals("Capitalizacion"))
                Third_filter_criptomonedas_byCapitalizacion(capitalizacion);
            else if (filter.Equals("Porcentaje"))
                Third_filter_criptomonedas_byPorcentaje(porcentaje);
            else
                Third_filter_criptomonedas_byRed(red);

            var criptomonedaRow = _driver.FindElements(By.Id("Criptomoneda_Nombre_" + nombre));

            //checks the expected row exists
            Assert.NotNull(criptomonedaRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(criptomonedaRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_5_alternate_flow_3_criptmonedasNotSelected()
        {
            //Arrange
            string expectedText = "Debes seleccionar al menos una Criptomoneda";

            //Act
            Precondition_perform_login();
            First_step_accessing_ventas();
            Second_step_accessing_link_Create_New();
            Third_alternate_not_selecting_criptomonedas();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("2", "2", "TarjetaCredito", "", "123", "12/12/2022", null, null, null, "Rellena el campo de 'Número de Tarjeta'")]
        [InlineData("2", "2", "TarjetaCredito", "1234567890123456", "", "12/12/2022", null, null, null, "Rellena el campo 'CVV'")]
        [InlineData("2", "2", "TarjetaCredito", "1234567890123456", "123", "", null, null, null, "Rellena el campo 'Fecha de Caducidad'")]
        [InlineData("2", "2", "PayPal", null, null, null, "", "967", "673240", "Rellena el campo 'Email'")]
        [InlineData("2", "2", "PayPal", null, null, null, "peter@uclm.com", "", "673240", "Rellena el campo 'Prefijo'")]
        [InlineData("2", "2", "PayPal", null, null, null, "peter@uclm.com", "967", "", "Rellena el campo 'Teléfono'")]
        [InlineData("", "2", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "")]
        [InlineData("400", "2", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "")]
        [InlineData("0", "0", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "Selecciona una cantidad")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6_UC1_6_15_alternate_flow_4_testingErrorsMandatorydata(string cantidadMoneda1, string cantidadMoneda2,
            string MetodoPago, string NumeroTarjeta, string CVV, string FechaCaducidad,
            string Email, string Prefijo, string Tlf, string expectedText)
        {

            //Act
            Precondition_perform_login();
            First_step_accessing_ventas();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(cantidadMoneda1, cantidadMoneda2, MetodoPago, NumeroTarjeta, CVV, FechaCaducidad, Email, Prefijo, Tlf);


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
            First_step_accessing_ventas();
            Second_step_accessing_link_Create_New();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }
    }
}