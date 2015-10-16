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

        [TestCase("10/15/2015", false)]
        [TestCase("10/14/2015", true)]
        public void GetOrdersFromDateTest(string input, bool expected)
        {
            DateTime convertedInput = DateTime.Parse(input);
            
            Response actual = oops.GetOrdersFromDate(convertedInput);

            Assert.AreEqual(expected, actual.Success);
        }

        //[Test]
        //public void FetchListOfStatesTest()
        //{
        //    var actual = oops.FetchListOfStates();

        //    Assert.AreEqual(true, actual.Count() != 0);
        //}
    }
}
