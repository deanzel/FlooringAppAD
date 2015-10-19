using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;
using FlooringApp.Models;

namespace FlooringApp.UI.WorkFlows
{
    public class RemoveOrderWorkFlow
    {
        private Order _orderToRemove;
        private OrderOperations _oops;
        private ErrorResponse _errorResponse;

        public RemoveOrderWorkFlow(OrderOperations oops)
        {
            _orderToRemove = new Order();
            _oops = oops;
            _errorResponse = new ErrorResponse();
        }

        public void Execute()
        {
            PromptForOrderDate();
            PromptForOrderNumber();
            FetchOrderInfoToRemove();

        }

        public void PromptForOrderDate()
        {
            bool validDate = false;

            do
            {
                Console.Clear();
                Console.Write("What is your Order Date? (MM/DD/YYYY): ");
                string dateInput = Console.ReadLine();
                Console.WriteLine();

                DateTime orderDate;

                if (dateInput == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter an Order Date.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                else if (DateTime.TryParse(dateInput, out orderDate))
                {
                    string input = "";
                    do
                    {
                        Console.Write("Is {0:d} the correct Order Date? (Y)es or (N)o: ", orderDate);
                        input = Console.ReadLine().ToUpper();

                    } while (input != "Y" && input != "N");

                    if (input == "Y")
                    {
                        _orderToRemove.OrderDate = orderDate;
                        validDate = true;
                    }
                }

                else
                {
                    _errorResponse.ErrorTime = DateTime.Now;
                    _errorResponse.ErrorSourceMethod = "Remove Order Method";
                    _errorResponse.Message = "Invalid date time";
                    _errorResponse.Input = dateInput;
                    _oops.SubmitErrorToLog(_errorResponse);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That is not a valid date.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

            } while (!validDate);
        }

        public void PromptForOrderNumber()
        {
            bool validOrderNumber = false;

            do
            {
                Console.Clear();
                Console.Write("What is your Order Number? ");
                string orderNumberInput = Console.ReadLine();
                Console.WriteLine();

                int orderNumber;

                if (orderNumberInput == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter an Order Number");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                else if (int.TryParse(orderNumberInput, out orderNumber))
                {
                    string input = "";
                    do
                    {
                        Console.Write("Is {0} the correct order number? (Y)es or (N)o: ", orderNumber);
                        input = Console.ReadLine().ToUpper();

                    } while (input != "Y" && input != "N");

                    if (input == "Y")
                    {
                        _orderToRemove.OrderNumber = orderNumber;
                        validOrderNumber = true;
                    }
                }

                else
                {
                    _errorResponse.ErrorTime = DateTime.Now;
                    _errorResponse.ErrorSourceMethod = "Remove Order Method";
                    _errorResponse.Message = "Invalid order number";
                    _errorResponse.Input = orderNumberInput;
                    _oops.SubmitErrorToLog(_errorResponse);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That is not a valid order number.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

            } while (!validOrderNumber);

        }

        public void FetchOrderInfoToRemove()
        {
            var response = _oops.GetOrderInfo(_orderToRemove);

            Console.Clear();

            if (response.Success)
            {
                Console.WriteLine("This is the information for the order you want to remove.");
                _orderToRemove.CustomerName = response.Order.CustomerName;
                _orderToRemove.State = response.Order.State;
                _orderToRemove.TaxRate = response.Order.TaxRate;
                _orderToRemove.ProductType = response.Order.ProductType;
                _orderToRemove.Area = response.Order.Area;
                _orderToRemove.CostPerSquareFoot = response.Order.CostPerSquareFoot;
                _orderToRemove.LaborCostPerSquareFoot = response.Order.LaborCostPerSquareFoot;
                _orderToRemove.MaterialCost = response.Order.MaterialCost;
                _orderToRemove.LaborCost = response.Order.LaborCost;
                _orderToRemove.Tax = response.Order.Tax;
                _orderToRemove.Total = response.Order.Total;

                DisplayOrderInfoToRemove();
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(response.Message);
                Console.WriteLine("Press ENTER to continue...");
                Console.ResetColor();
                Console.ReadLine();
            }
        }

        public void DisplayOrderInfoToRemove()
        {
            Console.WriteLine();
            Console.WriteLine("Order Number: --------- {0}", _orderToRemove.OrderNumber);
            Console.WriteLine("Order Date: ----------- {0:d}", _orderToRemove.OrderDate);
            Console.WriteLine("Customer Name: -------- {0}", _orderToRemove.CustomerName);
            Console.WriteLine("State: ---------------- {0}", _orderToRemove.State);
            Console.WriteLine("Tax Rate: ------------- {0}%", _orderToRemove.TaxRate);
            Console.WriteLine("Product Type: --------- {0}", _orderToRemove.ProductType);
            Console.WriteLine("Area: ----------------- {0:N}", _orderToRemove.Area);
            Console.WriteLine("Materials Rate: ------- {0:C}/sqft", _orderToRemove.CostPerSquareFoot);
            Console.WriteLine("Labor Rate: ------------{0:C}/sqft", _orderToRemove.LaborCostPerSquareFoot);
            Console.WriteLine("Materials Cost: ------- {0:C}", _orderToRemove.MaterialCost);
            Console.WriteLine("Labor Cost: ----------- {0:C}", _orderToRemove.LaborCost);
            Console.WriteLine("Tax: ------------------ {0:C}", _orderToRemove.Tax);
            Console.WriteLine("Total: ---------------- {0:C}", _orderToRemove.Total);
            Console.WriteLine();
            Console.WriteLine();

            PromptUserToConfirmRemoval();
        }

        public void PromptUserToConfirmRemoval()
        {
            string input;
            
            do
            {
                Console.Write("Do you want to remove this order? (Y)es or (N)o: ");
                input = Console.ReadLine().ToUpper();

            } while (input != "Y" && input != "N");

            if (input == "Y")
            {
                //Send to BLL for removal
                var response = _oops.SubmitRemoveOrderToRepo(_orderToRemove);
                Console.WriteLine();
                Console.WriteLine(response.Message);
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
        }

    }
}
   