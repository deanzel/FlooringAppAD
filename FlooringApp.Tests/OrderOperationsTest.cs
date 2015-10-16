using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;
using FlooringApp.Models;
using NUnit.Framework;

namespace FlooringApp.Tests
{
    [TestFixture]
    public class OrderOperationsTest
    {
        private OrderOperations oops;

        [SetUp]
        public void oopSetup()
        {
            oops = new OrderOperations();
        }

        [TestCase("06/03/2013", true)]
        [TestCase("10/16/2015", false)]
        [TestCase("10/17/2015", false)]
        public void GetOrdersFromDateTest(string input, bool expected)
        {
            DateTime convertedInput = DateTime.Parse(input);

            Response response = oops.GetOrdersFromDate(convertedInput);

            bool actual = response.Success;

            Assert.AreEqual(expected, actual);
        }


        //GetOrderInfo test case

    
    }
}
