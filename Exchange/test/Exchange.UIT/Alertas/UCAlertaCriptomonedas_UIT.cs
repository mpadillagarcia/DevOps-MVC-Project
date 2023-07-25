using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Xunit;

namespace Exchange.UIT.Alertas 
{
    public class UCAlertaCriptomonedas_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;

        public UCAlertaCriptomonedas_UIT()
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


        private void First_step_accessing_alertas()
        {
            _driver.FindElement(By.Id("AlertasController")).Click();

        }


        private void Second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("SeleccionCriptomonedasParaAlerta")).Click();
            //Alternatively we may use a linked text, i.e., any text that is between “<A>” and “</A”> tag
            //_driver.FindElement(By.LinkText("Create New")).Click();

        }

        private void Third_filter_criptomonedas_byNombre(string titleFilter)
        {
            _driver.FindElement(By.Id("criptomonedaNombre")).SendKeys(titleFilter);

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

        private void Third_filter_criptomonedas_byPorcentajeVariacion(string nombreFilter)
        {
            _driver.FindElement(By.Id("porcentajeVariacion")).SendKeys(nombreFilter);   

            _driver.FindElement(By.Id("filterbyNombreRed")).Click();              
        }

        private void Third_filter_criptomonedas_byCapitalizacion(string nombreFilter)
        {
            _driver.FindElement(By.Id("capitalizacion")).Clear();

            _driver.FindElement(By.Id("capitalizacion")).SendKeys(nombreFilter);  

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
        private void Fourth_fill_in_information_and_press_create(string precioAlertaCriptomoneda1,
            string precioAlertaCriptomoneda2, string fechaExpira)
        {

            _driver.FindElement(By.Id("FechaExpira")).SendKeys(fechaExpira);

            _driver.FindElement(By.Id("Criptomoneda_PrecioAlerta_2")).Clear();
            _driver.FindElement(By.Id("Criptomoneda_PrecioAlerta_2")).SendKeys(precioAlertaCriptomoneda1);

            _driver.FindElement(By.Id("Criptomoneda_PrecioAlerta_4")).Clear();
            _driver.FindElement(By.Id("Criptomoneda_PrecioAlerta_4")).SendKeys(precioAlertaCriptomoneda2);

            _driver.FindElement(By.Id("CreateButton")).Click();
        }

        [Theory]
        [ClassData(typeof(AlertaCriptomonedasTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_basic_flow(string precioAlertaCriptomoneda1,
            string precioAlertaCriptomoneda2, string fechaExpira)
        {
            //Arrange
            string[] expectedText = { "Details - Exchange","Details",
                "Alerta","Peter","Jackson","FechaAlerta",
                fechaExpira,"Bitcoin","BUSD"};
            //Act
            Precondition_perform_login();
            First_step_accessing_alertas();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(precioAlertaCriptomoneda1,
            precioAlertaCriptomoneda2, fechaExpira);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        /*
        [Fact(Skip = "As precondition, first execute script dbo.Movie.Quantity0 to update quantityforpurchase to 0")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_alternate_flow_1_NoCriptomonedasAvailable()
        {
            //Arrange
            string expectedText = "No hay criptomonedas disponibles";

            //Act
            Precondition_perform_login();
            First_step_accessing_alertas();
            Second_step_accessing_link_Create_New();

            var criptomonedaRow = _driver.FindElement(By.Id("NoCriptomonedas"));

            //checks the expected row exists
            Assert.NotNull(criptomonedaRow);
            Assert.Equal(expectedText, criptomonedaRow.Text);
        }
        */

        [Theory]
        [InlineData("Bitcoin", "5", "Red Bitcoin", "988000000", "-1", "Nombre")]
        [InlineData("BUSD", "1", "Red Binance", "11500000", "-0,05", "Red")]
        [InlineData("AXS", "2", "Red Ethereum", "56000000", "5", "Capitalizacion")]
        [InlineData("Wrapped Bitcoin", "3", "Red Bitcoin", "12000000", "-0,01", "PorcentajeVariacion")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4_alternate_flow_2_filteringbyNombre(string nombre, string precio,
            string red, string capitalizacion, string porcentajeVariacion, string filter)
        {
            //Arrange
            string[] expectedText = { nombre, precio, red, capitalizacion, porcentajeVariacion };

            //Act
            Precondition_perform_login();
            First_step_accessing_alertas();
            Second_step_accessing_link_Create_New();
            if (filter.Equals("Nombre"))
                Third_filter_criptomonedas_byNombre(nombre);
            if (filter.Equals("Capitalizacion"))
                Third_filter_criptomonedas_byCapitalizacion(capitalizacion);
            if (filter.Equals("PorcentajeVariacion"))
                Third_filter_criptomonedas_byPorcentajeVariacion(porcentajeVariacion);
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
            string expectedText = "Debes seleccionar al menos una criptomoneda";

            //Act
            Precondition_perform_login();
            First_step_accessing_alertas();
            Second_step_accessing_link_Create_New();
            Third_alternate_not_selecting_criptomonedas();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("", "2", "2", "Por favor, selecciona la Fecha de Expiración de la alerta")]
        [InlineData("30/12/2022", "", "2", "El campo de precio de alerta es obligatorio")]
        [InlineData("30/12/2022", "0", "0", "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6_UC1_6_15_alternate_flow_4_testingErrorsMandatorydata(string fechaExpira, string precioAlertaCriptomoneda1, string precioAlertaCriptomoneda2, string expectedText)
        {

            //Act
            Precondition_perform_login();
            First_step_accessing_alertas();
            Second_step_accessing_link_Create_New();
            Third_select_criptomonedas_and_submit();
            Fourth_fill_in_information_and_press_create(fechaExpira, precioAlertaCriptomoneda1, precioAlertaCriptomoneda2);


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
            First_step_accessing_alertas();
            Second_step_accessing_link_Create_New();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }
    }
}
