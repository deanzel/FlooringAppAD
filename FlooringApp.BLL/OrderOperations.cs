using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Data.OrderRepositories;
using FlooringApp.Models;

namespace FlooringApp.BLL
{
    public class OrderOperations
    {
        private IOrderRepository _orderRepo;

        public OrderOperations()
        {
            _orderRepo = OrderRepositoryFactory.CreateOrderRepository();
}

        public Response GetOrdersFromDate(DateTime orderDate)
        {
            var orders = _orderRepo.GetOrdersFromDate(orderDate);

            var response = new Response();

            if (orders.Count()==0)
            {
                response.Success = false;
                response.Message = "There are no orders on this date.";
            }
            else
            {
                response.Success = true;
                response.OrdersList = orders;
            }

            return response;
        }
    }
}
