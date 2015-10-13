using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Data.OrderRepositories;
using FlooringApp.Data.ProductsandTaxes;
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

        public Response FetchStateTaxInfo(string stateAbbreviation)
        {
            var stateTaxRepo = new TaxRepo();
            var response = new Response();

            var stateTaxInfo = stateTaxRepo.GetStateTaxInfo(stateAbbreviation);

            if (stateTaxInfo == null)
            {
                response.Success = false;
                response.Message = "That state is not in our database.";
            }
            else
            {
                response.Success = true;
                response.StateTaxInfo = stateTaxInfo;
            }

            return response;
        }

        public List<Product> FetchProductsList()
        {
            var productsRepo = new ProductsRepo();
            return productsRepo.GetProductsList();
        }

        public Response FetchProductInfo(string ProductType)
        {
            var response = new Response();
            var productsRepo = new ProductsRepo();

            var product = productsRepo.GetProductInfo(ProductType);

            if (product == null)
            {
                response.Success = false;
                response.Message = "That product is not in our database.";
            }
            else
            {
                response.Success = true;
                response.ProductInfo = product;
            }

            return response;
        }
    }
}
