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


<<<<<<< HEAD
        [TestCase("10/15/2015", false)]
        [TestCase("10/14/2015", true)]
        public void GetOrdersFromDateTest(string input, bool expected)
        {
            DateTime convertedInput = DateTime.Parse(input);

            Response actual = oops.GetOrdersFromDate(convertedInput);
        }

        [TestCase("06/03/2013", true)]
=======
        //GetOrdersFromDate Test Cases
>>>>>>> c86ba2460694d63fff947d65b8c49a917a8484ed
        [TestCase("06/01/2013", true)]
        [TestCase("10/12/2015", true)]
        [TestCase("10/16/2015", false)]
        [TestCase("10/17/2015", false)]
        public void GetOrdersFromDateTest(string input, bool expected)
        {
            DateTime convertedInput = DateTime.Parse(input);

            Response response = _oops.GetOrdersFromDate(convertedInput);

            bool actual = response.Success;

            Assert.AreEqual(expected, actual.Success);
        }

        //[Test]
        //public void FetchListOfStatesTest()
        //{
        //    var actual = oops.FetchListOfStates();

<<<<<<< HEAD
        //    Assert.AreEqual(true, actual.Count() != 0);
        //}
=======
        //GetOrderInfo Test Cases

        RandomNumberGenerator

    
>>>>>>> c86ba2460694d63fff947d65b8c49a917a8484ed
    }
}
