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
        OrderOperations oops;

        [SetUp]
        public void oopSetup()
        {
            oops = new OrderOperations();
        }

        [TestCase(DateTime.Now, {})]
        public void GetOrdersFromDateTest(DateTime input, Response expected)
        {
            Response actual = oops.GetOrdersFromDate(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
