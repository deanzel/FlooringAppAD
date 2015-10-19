using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;
using FlooringApp.Models;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace FlooringApp.Tests
{
    [TestFixture]
    public class OrderOperationsTest
    {
        private OrderOperations _oops;

        [SetUp]
        public void oopSetup()
        {
            _oops = new OrderOperations();
        }


        //Testing for our BLL called OrderOperations
        //Uses the Mock repository and test files must be copied from DataFiles\Mock folder to the Debug bin folder of
        //the OrderOpertaionsTest project folder. Our mock data is all externally stored in text files and loaded once in the beginning.
        
        //Testing GetOrdersFromDate()
        [TestCase("06/01/2013", true)]
        [TestCase("10/12/2015", true)]
        [TestCase("10/16/2015", false)]
        [TestCase("10/17/2015", false)]
        public void GetOrdersFromDateTest(string input, bool expected)
        {
            DateTime convertedInput = DateTime.Parse(input);

            Response response = _oops.GetOrdersFromDate(convertedInput);

            bool actual = response.Success;

            Assert.AreEqual(expected, actual);
        }
        
        //Testing FetchListOfStates() - number of states in the returned list
        [TestCase(4)]
        public void FetchListOfStatesTest(int expected)
        {
            var stateTaxList = _oops.FetchListOfStates();
            int result = stateTaxList.Count();

            Assert.AreEqual(expected, result);
        }

        //Testing FetchStateTaxInfo()
        //*Must run each test individually to pass due to "Item with the same key has already been added" error
        [TestCase("OH", 6.25)]
        [TestCase("PA", 6.75)]
        [TestCase("MI", 5.75)]
        [TestCase("IN", 6.00)]
        public void FetchStateTaxInfoTest(string input, decimal expected)
        {
            var response = _oops.FetchStateTaxInfo(input);
            decimal result = response.StateTaxInfo.TaxRate;

            Assert.AreEqual(expected, result);
        }

        //Testing FetchProductsList() - number of products in the returned list
        [TestCase(4)]
        public void FetchProductsListTest(int expected)
        {
            var productsList = _oops.FetchProductsList();
            int result = productsList.Count();

            Assert.AreEqual(expected, result);
        }

        //Testing FetchProductInfo()
        //*Must run each test individually to pass due to "Item with the same key has already been added" error
        [TestCase("Carpet", 2.25)]
        [TestCase("Laminate", 1.75)]
        [TestCase("Tile", 3.50)]
        [TestCase("Wood", 5.15)]

        public void FetchProductInfoTest(string input, decimal expected)
        {
            var response = _oops.FetchProductInfo(input);
            decimal result = response.ProductInfo.CostPerSquareFoot;

            Assert.AreEqual(expected, result);
        }
    }
}
