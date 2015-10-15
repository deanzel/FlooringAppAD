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
        public OrderOperations _oops;

        public RemoveOrderWorkFlow(OrderOperations oops)
        {
            _orderToRemove = new Order();
            _oops = oops;
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
                Console.Write("What date did you place the order on? (MM/DD/YYYY): ");
                string dateInput = Console.ReadLine();
                DateTime orderDate;

                if (DateTime.TryParse(dateInput, out orderDate))
                {
                    string input = "";
                    do
                    {
                        Console.Write("Is {0:d} the correct order date? (Y)es or (N)o: ", orderDate);
                        input = Console.ReadLine().ToUpper();

                        if (input != "Y" && input != "N")
                        {
                            Console.WriteLine("That is not a valid input. Press ENTER to continue.");
                            Console.ReadLine();
                        }
                    } while (input != "Y" && input != "N");

                    if (input == "Y")
                    {
                        _orderToRemove.OrderDate = orderDate;
                        validDate = true;
                    }
                    else
                    {
                        Console.WriteLine("OK. We will enter a new order date. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                }

                //failing DateTime TryParse
                else
                {
                    Console.WriteLine("That is not a valid date. Press ENTER to continue.");
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
                Console.Write("What is your order number? ");
                string orderNumberInput = Console.ReadLine();
                int orderNumber;

                if (int.TryParse(orderNumberInput, out orderNumber))
                {
                    string input = "";
                    do
                    {
                        Console.Write("Is {0} the correct order number? (Y)es or (N)o: ", orderNumber);
                        input = Console.ReadLine().ToUpper();
                        if (input != "Y" && input != "N")
                        {
                            Console.WriteLine("That is not a valid input. Press ENTER to continue.");
                            Console.ReadLine();
                        }

                        else if (input == "Y")
                        {
                            _orderToRemove.OrderNumber = orderNumber;
                            validOrderNumber = true;
                            
                        }
                        else
                        {
                            Console.WriteLine("OK. We will enter a new order number. Press ENTER to continue.");
                            Console.ReadLine();
                            
                        }
                    } while (input != "Y" && input != "N");
                }

                else
                {
                    Console.WriteLine("That is not a valid order number. Press ENTER to continue.");
                    Console.ReadLine();
                }

            } while (!validOrderNumber);
        }

        public void FetchOrderInfoToRemove()
        {
            var response = _oops.GetOrderInfo(_orderToRemove);

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
                
                Console.WriteLine();
                //prompt user if they want to remove
                PromptUserToConfirmRemoval();
            }
            else
            {
                Console.WriteLine("Error Occurred!!!");
                Console.WriteLine(response.Message);
                Console.WriteLine("Press ENTER to continue.");
                Console.ReadLine();
            }
        }

        public void DisplayOrderInfoToRemove()
        {
            Console.WriteLine();
            Console.WriteLine("Order Number: {0}", _orderToRemove.OrderNumber);
            Console.WriteLine("Order Date: {0:d}", _orderToRemove.OrderDate);
            Console.WriteLine("Customer Name: {0}", _orderToRemove.CustomerName);
            Console.WriteLine("State: {0}", _orderToRemove.State);
            Console.WriteLine("Tax Rate: {0}%", _orderToRemove.TaxRate);
            Console.WriteLine("Product Type: {0}", _orderToRemove.ProductType);
            Console.WriteLine("Area: {0}", _orderToRemove.Area);
            Console.WriteLine("Cost Per Square Foot: {0:C}", _orderToRemove.CostPerSquareFoot);
            Console.WriteLine("Labor Cost Per Square Foot: {0:C}", _orderToRemove.LaborCostPerSquareFoot);
            Console.WriteLine("Material Cost: {0:C}", _orderToRemove.MaterialCost);
            Console.WriteLine("Labor Cost: {0:C}", _orderToRemove.LaborCost);
            Console.WriteLine("Tax: {0:C}", _orderToRemove.Tax);
            Console.WriteLine("Total: {0:C}", _orderToRemove.Total);
            Console.WriteLine();
        }

        public void PromptUserToConfirmRemoval()
        {
            bool confirmRemoval = false;
            string input = "";

            Console.WriteLine();
            do
            {
                do
                {
                    Console.Write("Do you want to remove this order? (Y)es or (N)o: ");
                    input = Console.ReadLine().ToUpper();

                    if (input != "Y" && input != "N")
                    {
                        Console.WriteLine("Invalid input. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else if (input == "Y")
                    {
                        //Send to BLL for removal
                        var response = _oops.SubmitRemoveOrderToRepo(_orderToRemove);
                        Console.WriteLine();
                        Console.WriteLine(response.Message);
                        Console.WriteLine("Press ENTER to continue.");
                        Console.ReadLine();

                        confirmRemoval = true;
                    }
                    else
                    {
                        Console.WriteLine("OK. We will not remove the order and then we will return to the Main Menu. Press ENTER to continue.");
                        Console.ReadLine();
                        confirmRemoval = true;
                    }


                } while (input != "Y" && input != "N");

            } while (!confirmRemoval);
        }

    }
}