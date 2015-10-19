using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;
using FlooringApp.Models;

namespace FlooringApp.UI.WorkFlows
{
    public class DisplayOrdersWorkFlow
    {
        private DateTime _orderDate;
        private OrderOperations _oops;
        private ErrorResponse _errorResponse;

        public DisplayOrdersWorkFlow(OrderOperations oops)
        {
            _oops = oops;
            _errorResponse = new ErrorResponse();
        }
        public void Execute()
        {
            _orderDate = PromptOrderDateFromUser();
            
            DisplayOrdersFromDate();
        }

        public DateTime PromptOrderDateFromUser()
        {
            do
            {
                Console.Clear();
                Console.Write("Enter an order date (MM/DD/YYYY): ");
                string input = Console.ReadLine();
                DateTime orderDate;

                if (input == "")
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter an Order Date.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                else if (DateTime.TryParse(input, out orderDate))
                {
                    return orderDate;
                }

                else
                {
                    _errorResponse.ErrorTime = DateTime.Now;
                    _errorResponse.ErrorSourceMethod = "Display Order Method";
                    _errorResponse.Message = "Invalid date time";
                    _errorResponse.Input = input;
                    _oops.SubmitErrorToLog(_errorResponse);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("That was not a valid order date.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

            } while (true);
        }

        public void DisplayOrdersFromDate()
        {
            
            var response = _oops.GetOrdersFromDate(_orderDate);

            if (response.Success)
            {
                PrintOrdersFromDate(response.OrdersList);
                Console.ReadLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(response.Message);
                Console.WriteLine("Press Enter to continue...");
                Console.ResetColor();
                Console.ReadLine();
            }

        }

        public void PrintOrdersFromDate(List<Order> OrdersList)
        {
            Console.Clear();
            Console.WriteLine("Orders on the Date of {0:d}: ", _orderDate);

            foreach (var order in OrdersList)
            {
                Console.WriteLine();
                Console.WriteLine("Order Number ------ {0}", order.OrderNumber);
                Console.WriteLine("Customer Name ----- {0}", order.CustomerName);
                Console.WriteLine("State ------------- {0}", order.State);
                Console.WriteLine("Product Type ------ {0}", order.ProductType);
                Console.WriteLine("Materials Rate  --- {0:C}/sqft", order.CostPerSquareFoot);
                Console.WriteLine("Labor Rate -------- {0:C}/sqft", order.LaborCostPerSquareFoot);
                Console.WriteLine("Area -------------- {0} sqft", order.Area);
                Console.WriteLine("Material Cost ----- {0:C}", order.MaterialCost);
                Console.WriteLine("Labor Cost -------- {0:C}", order.LaborCost);
                Console.WriteLine("Tax Rate ---------- {0}%", order.TaxRate);
                Console.WriteLine("Tax --------------- {0:C}", order.Tax);
                Console.WriteLine("Total ------------- {0:C}", order.Total);
                Console.WriteLine();
            }

            Console.WriteLine("Press ENTER to continue...");
        }
    }
}
