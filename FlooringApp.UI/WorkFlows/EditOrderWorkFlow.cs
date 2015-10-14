using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;
using FlooringApp.Models;

namespace FlooringApp.UI.WorkFlows
{
    public class EditOrderWorkFlow
    {
        private Order _orderToEdit;

        private Order _orderPreEdit;

        public EditOrderWorkFlow()
        {
            _orderToEdit = new Order();
            _orderPreEdit = new Order();
        }

        public void Execute()
        {

          

            do
            {
                PromptForOrderDate();
                PromptForOrderNumber();
            } while (!FetchOrderInfoToEdit());

            do
            {
                //edit stuff prompts
                PromptForEditsFromUser();

                //Display new edits and calculations to confirm

            } while (!ConfirmEditsToOrder());

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
                        _orderToEdit.OrderDate = orderDate;
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
                            _orderToEdit.OrderNumber = orderNumber;
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

        public bool FetchOrderInfoToEdit()
        {
            var oops = new OrderOperations();

            var response = oops.GetOrderInfo(_orderToEdit);

            if (response.Success)
            {
                Console.WriteLine("This is the information for the order you want to edit.");
                _orderToEdit.CustomerName = response.Order.CustomerName;
                _orderToEdit.State = response.Order.State;
                _orderToEdit.TaxRate = response.Order.TaxRate;
                _orderToEdit.ProductType = response.Order.ProductType;
                _orderToEdit.Area = response.Order.Area;
                _orderToEdit.CostPerSquareFoot = response.Order.CostPerSquareFoot;
                _orderToEdit.LaborCostPerSquareFoot = response.Order.LaborCostPerSquareFoot;
                _orderToEdit.MaterialCost = response.Order.MaterialCost;
                _orderToEdit.LaborCost = response.Order.LaborCost;
                _orderToEdit.Tax = response.Order.Tax;
                _orderToEdit.Total = response.Order.Total;

                DisplayOrderInfo();

                string input = "";
                do
                {
                    Console.Write(
                        "Do you want to edit the Customer Name, State, Product Type, and/or Area in this order? (Y)es or (N)o: ");
                    input = Console.ReadLine().ToUpper();

                    if (input == "Y")
                    {
                        return true;
                    }
                    if (input == "N")
                    {
                        return false;
                    }
                    Console.WriteLine("That is an invalid entry. Press ENTER to continue.");
                    Console.ReadLine();
                } while (true);

            }
            else
            {
                Console.WriteLine("Error Occurred!!!");
                Console.WriteLine(response.Message);
                Console.WriteLine("Press ENTER to continue.");
                Console.ReadLine();
                return false;
            }
        }

        public void DisplayOrderInfo()
        {
            Console.WriteLine();
            Console.WriteLine("Order Number: {0}", _orderToEdit.OrderNumber);
            Console.WriteLine("Order Date: {0:d}", _orderToEdit.OrderDate);
            Console.WriteLine("Customer Name: {0}", _orderToEdit.CustomerName);
            Console.WriteLine("State: {0}", _orderToEdit.State);
            Console.WriteLine("Tax Rate: {0}%", _orderToEdit.TaxRate);
            Console.WriteLine("Product Type: {0}", _orderToEdit.ProductType);
            Console.WriteLine("Area: {0}", _orderToEdit.Area);
            Console.WriteLine("Cost Per Square Foot: {0:C}", _orderToEdit.CostPerSquareFoot);
            Console.WriteLine("Labor Cost Per Square Foot: {0:C}", _orderToEdit.LaborCostPerSquareFoot);
            Console.WriteLine("Material Cost: {0:C}", _orderToEdit.MaterialCost);
            Console.WriteLine("Labor Cost: {0:C}", _orderToEdit.LaborCost);
            Console.WriteLine("Tax: {0:C}", _orderToEdit.Tax);
            Console.WriteLine("Total: {0:C}", _orderToEdit.Total);
            Console.WriteLine();
        }

        public void PromptForEditsFromUser()
        {
            bool validState = false;
            bool validProductType = false;

            Console.Clear();
            Console.WriteLine(
                "OK, we will now edit the Customer Name, State, Product Type, and Area fields one by one." +
                "\nThe current order info will be inside parentheses." +
                "\nLeave the field blank and press ENTER if you don't want to edit it." +
                "\nPress ENTER to continue.");
            Console.ReadLine();

            //Prompt for Name
            Console.Clear();
            Console.Write("Enter Customer Name ({0}): ", _orderToEdit.CustomerName);
            _orderPreEdit.CustomerName = Console.ReadLine();

            //Prompt for State
            do
            {
                string stateInput = "";

                do
                {
                    Console.Clear();
                    Console.Write("Enter State Abbreviation ({0}): ", _orderToEdit.State);
                    stateInput = Console.ReadLine();
                    if (stateInput.Length != 2 && stateInput != "")
                    {
                        Console.WriteLine("That is not two letters. Press ENTER to continue.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                } while (stateInput.Length != 2 && stateInput != "");

                if (stateInput != "")
                {

                    var oops = new OrderOperations();
                    var response = oops.FetchStateTaxInfo(stateInput);

                    if (response.Success)
                    {
                        _orderPreEdit.State = response.StateTaxInfo.StateAbbreviation;
                        _orderPreEdit.TaxRate = response.StateTaxInfo.TaxRate;
                        validState = true;
                    }
                    else
                    {
                        Console.WriteLine("Error occured!!");
                        Console.WriteLine(response.Message);
                        Console.ReadLine();
                    }
                }
                else if (stateInput == "")
                {
                    validState = true;
                }
            } while (!validState);

            //Prompt for Product Type
            Console.WriteLine("Now we will edit Product Type.");
            do
            {
                var oops = new OrderOperations();
                string productInput = "";
                string input = "";

                Console.Clear();

                do
                {
                    Console.Write("Would you like to see the products list? (Y)es or (N)o: ");
                    input = Console.ReadLine().ToUpper();

                    if (input != "Y" && input != "N")
                    {
                        Console.WriteLine("That is not a valid input");
                    }
                    if (input == "Y")
                    {
                        var productsList = oops.FetchProductsList();
                        Console.Clear();
                        Console.WriteLine(
                            "Here is a list of our products, CostPerSquareFoot, and LaborCostPerSquareFoot");

                        foreach (var product in productsList)
                        {
                            Console.WriteLine("{0}:", product.ProductType);
                            Console.WriteLine("-Cost/sqft: {0:c}", product.CostPerSquareFoot);
                            Console.WriteLine("-Labor cost/sqft: {0:c}", product.LaborCostPerSquareFoot);
                            Console.WriteLine();
                        }
                    }
                } while (input != "Y" && input != "N");
                Console.Clear();
                Console.Write("Enter Product Type ({0}): ", _orderToEdit.ProductType);
                productInput = Console.ReadLine();

                if (productInput != "")
                {
                    var response = oops.FetchProductInfo(productInput);
                    if (response.Success)
                    {
                        _orderPreEdit.ProductType = response.ProductInfo.ProductType;
                        _orderPreEdit.CostPerSquareFoot = response.ProductInfo.CostPerSquareFoot;
                        _orderPreEdit.LaborCostPerSquareFoot = response.ProductInfo.LaborCostPerSquareFoot;
                        validProductType = true;
                    }
                    else
                    {
                        Console.WriteLine("Error Occurred!!!");
                        Console.WriteLine(response.Message);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                    }
                }
                else if (productInput == "")
                {
                    validProductType = true;
                }

            } while (!validProductType);

            //Prompt to edit Area
            string areaInputString = "";
            int areaInputInt;
            bool validInt = false;

            Console.Clear();
            do
            {
                Console.Write("Enter Area in sqft ({0}): ", _orderToEdit.Area);
                areaInputString = Console.ReadLine();
                if (areaInputString != "")
                {

                    if (int.TryParse(areaInputString, out areaInputInt))
                    {
                        if (areaInputInt > 0)
                        {
                            _orderPreEdit.Area = areaInputInt;
                            validInt = true;
                        }
                        else
                        {
                            Console.WriteLine("You must enter a value greater than 0!! Press ENTER to continue.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("That is not an integer. Press ENTER to continue.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else
                {
                    _orderPreEdit.Area = 0;
                    validInt = true;
                }
            } while (!validInt);
        }

        public bool ConfirmEditsToOrder()
        {
            Console.Clear();
            
            
            //Find and set what needs to be changed while printing the changes out.

            Order orderEditPreview = new Order();
            if (_orderPreEdit.CustomerName != null)
            {
                orderEditPreview.CustomerName = _orderPreEdit.CustomerName;
                Console.WriteLine("You changed Customer Name from {0} to {1}.", _orderToEdit.CustomerName, orderEditPreview.CustomerName);
            }
            else
            {
                orderEditPreview.CustomerName = _orderToEdit.CustomerName;
            }

            if (_orderPreEdit.State != null)
            {
                orderEditPreview.State = _orderPreEdit.State;
                orderEditPreview.TaxRate = _orderPreEdit.TaxRate;
                Console.WriteLine("You changed the State from {0} to {1}.", _orderToEdit.State, orderEditPreview.State);
            }
            else
            {
                orderEditPreview.State = _orderToEdit.State;
                orderEditPreview.TaxRate = _orderToEdit.TaxRate;
            }

            if (_orderPreEdit.ProductType != null)
            {
                orderEditPreview.ProductType = _orderPreEdit.ProductType;
                orderEditPreview.CostPerSquareFoot = _orderPreEdit.CostPerSquareFoot;
                orderEditPreview.LaborCostPerSquareFoot = _orderPreEdit.LaborCostPerSquareFoot;
                Console.WriteLine("You changed the Product Type from {0} to {1}.", _orderToEdit.ProductType, orderEditPreview.ProductType);
            }
            else
            {
                orderEditPreview.ProductType = _orderToEdit.ProductType;
                orderEditPreview.CostPerSquareFoot = _orderToEdit.CostPerSquareFoot;
                orderEditPreview.LaborCostPerSquareFoot = _orderToEdit.LaborCostPerSquareFoot;
            }

            if (_orderPreEdit.Area != 0)
            {
                orderEditPreview.Area = _orderPreEdit.Area;
                Console.WriteLine("You changed the Area from {0} to {1}.", _orderToEdit.Area, orderEditPreview.Area);
            }
            else
            {
                orderEditPreview.Area = _orderToEdit.Area;
            }

            //calculate new total prices
            orderEditPreview.MaterialCost = orderEditPreview.CostPerSquareFoot*orderEditPreview.Area;
            orderEditPreview.LaborCost = orderEditPreview.LaborCostPerSquareFoot*orderEditPreview.Area;
            decimal subtotalPreview = orderEditPreview.MaterialCost + orderEditPreview.LaborCost;
            orderEditPreview.Tax = subtotalPreview*(orderEditPreview.TaxRate/100);
            orderEditPreview.Total = subtotalPreview + orderEditPreview.Tax;

            orderEditPreview.OrderDate = _orderToEdit.OrderDate;
            orderEditPreview.OrderNumber = _orderToEdit.OrderNumber;

            //Display updated order preview
            Console.WriteLine();
            Console.WriteLine("Here is your new updated order info summary with new price calculations:");
            Console.WriteLine();
            Console.WriteLine("Order Number: {0}", orderEditPreview.OrderNumber);
            Console.WriteLine("Order Date: {0:d}", orderEditPreview.OrderDate);
            Console.WriteLine("Customer Name: {0}", orderEditPreview.CustomerName);
            Console.WriteLine("State: {0}", orderEditPreview.State);
            Console.WriteLine("Product Type: {0}", orderEditPreview.ProductType);
            Console.WriteLine("Area : {0} sqft", orderEditPreview.Area);
            Console.WriteLine("Materials Cost: {0:c}", orderEditPreview.MaterialCost);
            Console.WriteLine("Labor Cost: {0:c}", orderEditPreview.LaborCost);
            Console.WriteLine("Subtotal: {0:c}", orderEditPreview.Total - orderEditPreview.Tax);
            Console.WriteLine("Tax: {0:c}", orderEditPreview.Tax);
            Console.WriteLine("TOTAL COST: {0:c}", orderEditPreview.Total);
            Console.WriteLine();

            string input = "";
            do
            {
                Console.Write("Is this new order info accurate? (Y)es or (N)o: ");
                input = Console.ReadLine().ToUpper();
                if (input != "Y" && input != "N")
                {
                    Console.WriteLine("Invalid input. Press ENTER to continue.");
                    Console.ReadLine();
                }
            } while (input != "Y" && input != "N");

            if (input == "Y")
            {
                //Run method and updated orderInfo to BLL with orderEditPreview
                var oops = new OrderOperations();
                var response = oops.SubmitEditOrderToRepo(orderEditPreview);
                Console.WriteLine(response.Message);
                Console.ReadLine();
                return true;

            }
            else
            {
                return false;
            }

        }

    }
}
