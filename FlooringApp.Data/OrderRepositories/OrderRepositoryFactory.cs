using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Models;

namespace FlooringApp.Data.OrderRepositories
{
    public static class OrderRepositoryFactory
    {
        public static IOrderRepository CreateOrderRepository()
        {
            string initialBuild = ConfigurationManager.AppSettings["initialBuild"];

            switch (ConfigurationManager.AppSettings["mode"])
            {
                case "prod":
                    return new ProdOrderRepository(initialBuild);
                case "mock":
                    return new MockOrderRepository(initialBuild);
                default:
                    throw new NotSupportedException();
            }
        }

    }
}
