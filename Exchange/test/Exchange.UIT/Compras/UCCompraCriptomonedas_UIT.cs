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


namespace Exchange.UIT.Compras
{
    public class UCCompraCriptomonedas_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;

        public UCCompraCriptomonedas_UIT()
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


        private void First_step_accessing_purchases()
        {
            _driver.FindElement(By.Id("CompraController")).Click();

        }


        private void Second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("SeleccionCriptomonedasParaCompra")).Click();
            //Alternatively we may use a linked text, i.e., any text that is between “<A>” and “</A”> tag
            //_driver.FindElement(By.LinkText("Create New")).Click();

        }

        private void Third_filter_criptomonedas_byNombre(string nombreFilter)
        {
            _driver.FindElement(By.Id("criptomonedaNombre")).SendKeys(nombreFilter);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();
        }
        private void Third_filter_criptomonedas_byPrecio(string precioFilter)
        {
            _driver.FindElement(By.Id("Precio")).Clear();
            _driver.FindElement(By.Id("Precio")).SendKeys(precioFilter);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();
        }
        private void Third_filter_criptomonedas_byPorcentajeVariacion(string porcentajeFilter)
        {
            _driver.FindElement(By.Id("PorcentajeVariacion")).SendKeys(porcentajeFilter);

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();
        }
        private void Third_filter_criptomonedas_byRed(string redSeleccionada)
        {

            var red = _driver.FindElement(By.Id("criptomonedaRedSeleccionada"));

            //create select element object 
            SelectElement selectElement = new SelectElement(red);
            //select Action from the dropdown menu
            selectElement.SelectByText(redSeleccionada);

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
        private void Fourth_fill_in_information_and_press_create(string quantityCriptomoneda1,
            string quantityCriptomoneda2, string MetodoPago, string NumeroTarjeta, string CVV, string FechaExpiracion,
            string Email, string Prefijo, string Tlf)
        {

            _driver.FindElement(By.Id("Movie_Quantity_2")).Clear();
            _driver.FindElement(By.Id("Movie_Quantity_2")).SendKeys(quantityCriptomoneda1);

            _driver.FindElement(By.Id("Movie_Quantity_4")).Clear();
            _driver.FindElement(By.Id("Movie_Quantity_4")).SendKeys(quantityCriptomoneda2);

            if (MetodoPago.Equals("TarjetaCredito"))
            {
                _driver.FindElement(By.Id("r11")).Click();

                _driver.FindElement(By.Id("NumeroTarjeta")).SendKeys(NumeroTarjeta);

                _driver.FindElement(By.Id("CVV")).SendKeys(CVV);

                _driver.FindElement(By.Id("FechaExpiracion")).Clear();
                _driver.FindElement(By.Id("FechaExpiracion")).SendKeys(FechaExpiracion);
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
        [ClassData(typeof(CompraCriptomonedasTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_basic_flow(string quantityCriptomoneda1,
            string quantityCriptomoneda2, string MetodoPago, string NumeroTarjeta, string CVV, string FechaExpiracion,
            string Email, string Prefijo, string Tlf)
        {
            //Arrange
            string[] expectedText = { "Details - Exchange","Details",
                "Compra","Peter","Jackson","CompraFecha",
                "PrecioTotal","Bitcoin","BUSD"};
            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(quantityCriptomoneda1, quantityCriptomoneda2, MetodoPago, NumeroTarjeta, CVV, FechaExpiracion, Email, Prefijo, Tlf);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        //[Fact(Skip = "As precondition, first execute script dbo.Movie.Quantity0 to update quantityforpurchase to 0")]
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_alternate_flow_1_NoCriptomonedasAvailable()
        {
            //Arrange
            string expectedText = "No hay criptomonedas disponibles";

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();

            var criptomonedaRow = _driver.FindElement(By.Id("NoCriptomonedas"));

            //checks the expected row exists
            Assert.NotNull(criptomonedaRow);
            Assert.Equal(expectedText, criptomonedaRow.Text);
        }


        [Theory]
        [InlineData("Bitcoin", "52353", "-1", "Red Bitcoin", "Nombre")]
        [InlineData("BUSD", "1", "-0,05", "Red Binance", "Red")]
        [InlineData("Bitcoin", "52353", "-1", "Red Bitcoin", "Precio")]
        [InlineData("BUSD", "1", "-0,05", "Red Binance", "Porcentaje")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4_alternate_flow_2_filteringbyTitle(string nombre, string precio,
            string porcentaje, string red, string filter)
        {
            //Arrange
            string[] expectedText = { nombre, precio, porcentaje, red };

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            if (filter.Equals("Nombre"))
                Third_filter_criptomonedas_byNombre(nombre);
            else if (filter.Equals("Precio"))
                Third_filter_criptomonedas_byPrecio(precio);
            else if (filter.Equals("Porcentaje"))
                Third_filter_criptomonedas_byPorcentajeVariacion(porcentaje);
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
        public void UC1_5_alternate_flow_3_criptomonedasNotSelected()
        {
            //Arrange
            string expectedText = "Necesitas seleccionar al menos una criptomoneda";

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            Third_alternate_not_selecting_criptomonedas();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("2", "2", "TarjetaCredito", "", "123", "12/12/2022", null, null, null, "Porfavor, Rellena el campo Numero de Tarjeta")]
        [InlineData("2", "2", "TarjetaCredito", "1234567890123456", "", "12/12/2022", null, null, null, "Porfavor, Rellena el campo CVV")]
        [InlineData("2", "2", "TarjetaCredito", "1234567890123456", "123", "", null, null, null, "Porfavor, Rellena el campo Fecha de expiracion")]
        [InlineData("2", "2", "PayPal", null, null, null, "", "967", "673240", "Porfavor, Rellena el campo Email")]
        [InlineData("2", "2", "PayPal", null, null, null, "peter@uclm.com", "", "673240", "Porfavor, Rellena el campo Prefijo")]
        [InlineData("2", "2", "PayPal", null, null, null, "peter@uclm.com", "967", "", "Porfavor, Rellena el campo Tlf")]
        [InlineData("", "2", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "")]
        [InlineData("400", "2", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null,"")]
        [InlineData("0", "0", "TarjetaCredito", "1234567890123456", "123", "12/12/2022", null, null, null, "Porfavor, Selecciona una cantidad mayor que 0")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6_UC1_6_15_alternate_flow_4_testingErrorsMandatorydata(string quantityCriptomoneda1, string quantityCriptomoneda2,
            string MetodoPago, string NumeroTarjeta, string CVV, string FechaExpiracion,
            string Email, string Prefijo, string Tlf, string expectedText)
        {

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(quantityCriptomoneda1, quantityCriptomoneda2, MetodoPago, NumeroTarjeta, CVV, FechaExpiracion, Email, Prefijo, Tlf);


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
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }
    }
}