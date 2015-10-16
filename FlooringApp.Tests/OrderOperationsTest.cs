using System;
using System.Collections.Generic;
using System.Linq;
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
        private OrderOperations oops;

        [SetUp]
        public void oopSetup()
        {
            oops = new OrderOperations();
        }

<<<<<<< HEAD
        [TestCase("10/15/2015", false)]
        [TestCase("10/14/2015", true)]
        public void GetOrdersFromDateTest(string input, bool expected)
        {
            DateTime convertedInput = DateTime.Parse(input);
            
            Response actual = oops.GetOrdersFromDate(convertedInput);
=======
        [TestCase("06/03/2013", true)]
        [TestCase("10/16/2015", false)]
        [TestCase("10/17/2015", false)]
        public void GetOrdersFromDateTest(string input, bool expected)
        {
            DateTime convertedInput = DateTime.Parse(input);

            Response response = oops.GetOrdersFromDate(convertedInput);

            bool actual = response.Success;
>>>>>>> 0264cd288cd684b2a786075cbb155616deee8774

            Assert.AreEqual(expected, actual.Success);
        }

<<<<<<< HEAD
        //[Test]
        //public void FetchListOfStatesTest()
        //{
        //    var actual = oops.FetchListOfStates();

        //    Assert.AreEqual(true, actual.Count() != 0);
        //}
=======

        //GetOrderInfo test case

    
>>>>>>> 0264cd288cd684b2a786075cbb155616deee8774
    }
}
