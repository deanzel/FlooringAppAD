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
            switch (ConfigurationManager.AppSettings["mode"])
            {
                case "prod":
                    return new ProdOrderRepository();
                default: //"mock"
                    return new MockOrderRepository();
            }
        }

    }
}
