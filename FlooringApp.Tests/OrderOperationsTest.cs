using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private OrderOperations _oops;

        [SetUp]
        public void oopSetup()
        {
            _oops = new OrderOperations();
        }


        //GetOrdersFromDate Test Cases
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


        //GetOrderInfo Test Cases

        RandomNumberGenerator

    
    }
}
