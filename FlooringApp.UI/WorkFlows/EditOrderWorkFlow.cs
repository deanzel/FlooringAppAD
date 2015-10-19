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
        private bool exitToMainMenu;
        private OrderOperations _oops;
        private ErrorResponse _errorResponse;
        private int _changesCount;

        public EditOrderWorkFlow(OrderOperations oops)
        {
            _orderToEdit = new Order();
            _errorResponse = new ErrorResponse();
            _oops = oops;
            
        }

        public void Execute()
        {
            do
            {
                exitToMainMenu = false;
                PromptForOrderDate();
                PromptForOrderNumber();
            } while (!FetchOrderInfoToEdit());

            if (!exitToMainMenu)
            {
                do
                {
                    _changesCount = 0;
                    _orderPreEdit = new Order();

                    PromptForEditsFromUser();

                } while (!ConfirmEditsToOrder());
            }
        }

        public void PromptForOrderDate()
        {
            string input = "";

            do
            {
                Console.Clear();
                Console.Write("What date did you place the order on? (MM/DD/YYYY): ");
                string dateInput = Console.ReadLine();
                DateTime orderDate;

                if (dateInput == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("You must enter a Date Time.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();

                }

                else if (DateTime.TryParse(dateInput, out orderDate))
                {
                    Console.WriteLine();

                    do
                    {
                        Console.Write("Is {0:d} the correct order date? (Y)es or (N)o: ", orderDate);
                        input = Console.ReadLine().ToUpper();
                        
                    } while (input != "Y" && input != "N");

                    if (input == "Y")
                    {
                        _orderToEdit.OrderDate = orderDate;
                    }
                }
                
                else
                {
                    _errorResponse.ErrorTime = DateTime.Now;
                    _errorResponse.ErrorSourceMethod = "Edit Order Method";
                    _errorResponse.Message = "Invalid date time";
                    _errorResponse.Input = dateInput;
                    _oops.SubmitErrorToLog(_errorResponse);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("That is not a valid date.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

            } while (input != "Y");

        }

        public void PromptForOrderNumber()
        {
            string input = "";
            int orderNumber;

            do
            {
                Console.Clear();
                Console.Write("What is your order number? ");
                string orderNumberInput = Console.ReadLine();

                if (orderNumberInput == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("You must enter an Order Number.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                else if (int.TryParse(orderNumberInput, out orderNumber))
                {
                    Console.WriteLine();

                    do
                    {
                        Console.Write("Is {0} the correct order number? (Y)es or (N)o: ", orderNumber);
                        input = Console.ReadLine().ToUpper();

                    } while (input != "Y" && input != "N");

                    if (input == "Y")
                    {
                        _orderToEdit.OrderNumber = orderNumber;
                    }
                }

                else
                {
                    _errorResponse.ErrorTime = DateTime.Now;
                    _errorResponse.ErrorSourceMethod = "Edit Order Method";
                    _errorResponse.Message = "Invalid order number";
                    _errorResponse.Input = orderNumberInput;
                    _oops.SubmitErrorToLog(_errorResponse);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("That is not a valid order number.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

            } while (input != "Y");
        }

        public bool FetchOrderInfoToEdit()
        {
            var response = _oops.GetOrderInfo(_orderToEdit);
            string input = "";

            if (response.Success)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Order to modify:");
                    Console.WriteLine();

                    response.Order.OrderDate = _orderToEdit.OrderDate;
                    _orderToEdit = response.Order;

                    DisplayOrderInfo();

                    Console.WriteLine("Is this the correct order?");
                    Console.Write("(Y)es, (N)o, or (Q)uit: ");
                    input = Console.ReadLine().ToUpper();

                    if (input == "Y")
                    {
                        return true;
                    }
                    if (input == "N")
                    {
                        return false;
                    }
                    if (input == "Q")
                    {
                        exitToMainMenu = true;
                        return true;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("That is an invalid entry.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();

                } while (true);

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(response.Message);
                Console.WriteLine("Press ENTER to continue...");
                Console.ResetColor();
                Console.ReadLine();
                return false;
            }
        }

        public void DisplayOrderInfo()
        {
            Console.WriteLine("Order Number ----- {0}", _orderToEdit.OrderNumber);
            Console.WriteLine("Order Date ------- {0:d}", _orderToEdit.OrderDate);
            Console.WriteLine("Customer Name ---- {0}", _orderToEdit.CustomerName);
            Console.WriteLine("State ------------ {0}", _orderToEdit.State);
            Console.WriteLine("Tax Rate --------- {0}%", _orderToEdit.TaxRate);
            Console.WriteLine("Product Type ----- {0}", _orderToEdit.ProductType);
            Console.WriteLine("Area ------------- {0:N} sqft", _orderToEdit.Area);
            Console.WriteLine("Materials Rate --- {0:C}/sqft", _orderToEdit.CostPerSquareFoot);
            Console.WriteLine("Labor Rate ------- {0:C}/sqft", _orderToEdit.LaborCostPerSquareFoot);
            Console.WriteLine("Material Cost ---- {0:C}", _orderToEdit.MaterialCost);
            Console.WriteLine("Labor Cost ------- {0:C}", _orderToEdit.LaborCost);
            Console.WriteLine("Tax -------------- {0:C}", _orderToEdit.Tax);
            Console.WriteLine("Total ------------ {0:C}", _orderToEdit.Total);
            Console.WriteLine();
        }

        public void PromptForEditsFromUser()
        {
            Console.Clear();
            Console.WriteLine("INSTRUCTIONS:");
            Console.WriteLine("-The current order info will be inside parentheses.");
            Console.WriteLine("-Leave the field blank and press ENTER if you don't want to edit it.");
            Console.WriteLine();
            Console.WriteLine("Press ENTER to begin editing.");
            Console.ReadLine();

            bool validState = false;
            bool validProductType = false;
            bool validName = false;

            //Prompt for Name
            do
            {
                
                Console.Clear();
                Console.Write("Enter Customer Name ({0}): ", _orderToEdit.CustomerName);
                string nameInput = Console.ReadLine();
                Console.WriteLine();

                string validInput;

                if (nameInput != "")
                {
                    if (nameInput == _orderToEdit.CustomerName)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your Name already is {0}.", _orderToEdit.CustomerName);
                        Console.WriteLine("To keep the same Customer Name, press ENTER during the prompt.");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                    else
                    {
                        do
                        {
                            Console.Write("Are you sure you want to change your name to {0}? (Y)es or (N)o: ",
                                nameInput);
                            validInput = Console.ReadLine().ToUpper();

                        } while (validInput != "Y" && validInput != "N");

                        if (validInput == "Y")
                        {
                            _orderPreEdit.CustomerName = nameInput;
                            validName = true;
                            _changesCount++;
                        }
                    }
                }

                else
                {
                    validName = true;
                }

            } while (!validName);

            //Prompt for State
            do
            {
                string stateInput = "";

                do
                {
                    Console.Clear();
                    Console.Write("Enter State Abbreviation ({0}): ", _orderToEdit.State);
                    stateInput = Console.ReadLine();
                    Console.WriteLine();

                    if (stateInput.Length != 2 && stateInput != "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That is not two letters. Press ENTER to continue.");
                        Console.ReadLine();
                        Console.ResetColor();
                    }

                } while (stateInput.Length != 2 && stateInput != "");

                if (stateInput != "")
                {
                    if (stateInput.ToUpper() == _orderToEdit.State)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your State already is {0}.", _orderToEdit.State);
                        Console.WriteLine("To keep the same State, press ENTER during the prompt.");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                    else
                    {
                        string confirmation;
                        do
                        {
                            Console.Write("Are you sure you want to change your state to {0}? (Y)es or (N)o: ",
                                stateInput);
                            confirmation = Console.ReadLine().ToUpper();

                        } while (confirmation != "Y" && confirmation != "N");

                        if (confirmation == "Y")
                        {


                            var response = _oops.FetchStateTaxInfo(stateInput);

                            if (response.Success)
                            {
                                _orderPreEdit.State = response.StateTaxInfo.StateAbbreviation;
                                _orderPreEdit.TaxRate = response.StateTaxInfo.TaxRate;
                                validState = true;
                                _changesCount++;
                            }

                            else
                            {
                                _errorResponse.ErrorTime = DateTime.Now;
                                _errorResponse.ErrorSourceMethod = "Edit Order Method";
                                _errorResponse.Message = "Invalid state input";
                                _errorResponse.Input = stateInput;
                                _oops.SubmitErrorToLog(_errorResponse);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine();
                                Console.WriteLine(response.Message);
                                Console.ResetColor();
                                Console.ReadLine();
                            }
                        }
                    }
                }

                else if (stateInput == "")
                {
                    validState = true;
                }

            } while (!validState);

            //Prompt for Product Type
            
            Console.Clear();
            Console.WriteLine("Now we will edit Product Type.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.Clear();

            string input;
            do
            {
                Console.Write("Would you like to see the products list? (Y)es or (N)o: ");
                input = Console.ReadLine().ToUpper();

            } while (input != "Y" && input != "N");

            do
            {
                Console.Clear();

                string productInput = "";

                if (input == "Y")
                {
                    var productsList = _oops.FetchProductsList();
                    
                    Console.WriteLine(
                        "Here is a list of our Products, Materials Cost Rate, and Labor Cost Rate: ");

                    foreach (var product in productsList)
                    {
                        Console.WriteLine("{0}:", product.ProductType);
                        Console.WriteLine("-Cost/sqft: {0:c}", product.CostPerSquareFoot);
                        Console.WriteLine("-Labor cost/sqft: {0:c}", product.LaborCostPerSquareFoot);
                        Console.WriteLine();
                    }
                }
                
                Console.Write("Enter Product Type ({0}): ", _orderToEdit.ProductType);
                productInput = Console.ReadLine();

                if (productInput != "")
                {
                    if (productInput.ToUpper() == _orderToEdit.ProductType)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Your Product Type already is {0}.", _orderToEdit.ProductType);
                        Console.WriteLine("To keep the same Product Type, press ENTER during the prompt.");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                    }

                    else
                    {
                        var response = _oops.FetchProductInfo(productInput);

                        if (response.Success)
                        {
                            _orderPreEdit.ProductType = response.ProductInfo.ProductType;
                            _orderPreEdit.CostPerSquareFoot = response.ProductInfo.CostPerSquareFoot;
                            _orderPreEdit.LaborCostPerSquareFoot = response.ProductInfo.LaborCostPerSquareFoot;
                            validProductType = true;
                            _changesCount++;
                        }

                        else
                        {
                            _errorResponse.ErrorTime = DateTime.Now;
                            _errorResponse.ErrorSourceMethod = "Edit Order Method";
                            _errorResponse.Message = "Invalid product entry";
                            _errorResponse.Input = productInput;
                            _oops.SubmitErrorToLog(_errorResponse);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine();
                            Console.WriteLine(response.Message);
                            Console.WriteLine("Press ENTER to continue...");
                            Console.ResetColor();
                            Console.ReadLine();
                        }
                    }
                }

                else if (productInput == "")
                {
                    validProductType = true;
                }

            } while (!validProductType);

            //Prompt to edit Area
            bool validInt = false;
            string areaInputString = "";

            do
            {
                Console.Clear();
                Console.Write("Enter Area in sqft ({0}): ", _orderToEdit.Area);
                areaInputString = Console.ReadLine();
                if (areaInputString != "")
                {
                    
                        decimal areaInputInt;

                        if (decimal.TryParse(areaInputString, out areaInputInt))
                        {
                            if (areaInputInt == _orderToEdit.Area)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine();
                                Console.WriteLine("Your Area already is {0} sqft.", _orderToEdit.Area);
                                Console.WriteLine("To keep the same Area, press ENTER during the prompt.");
                                Console.WriteLine("Press ENTER to continue...");
                                Console.ResetColor();
                                Console.ReadLine();
                            }
                            else if (areaInputInt > 0)
                            {
                                _orderPreEdit.Area = areaInputInt;
                                validInt = true;
                                _changesCount++;
                            }
                            else
                            {
                                _errorResponse.ErrorTime = DateTime.Now;
                                _errorResponse.ErrorSourceMethod = "Edit Order Method";
                                _errorResponse.Message = "Negative area input";
                                _errorResponse.Input = areaInputString;
                                _oops.SubmitErrorToLog(_errorResponse);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine();
                                Console.WriteLine("You must enter a value greater than.");
                                Console.WriteLine("Press ENTER to continue...");
                                Console.ResetColor();
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            _errorResponse.ErrorTime = DateTime.Now;
                            _errorResponse.ErrorSourceMethod = "Edit Order Method";
                            _errorResponse.Message = "Invalid area input";
                            _errorResponse.Input = areaInputString;
                            _oops.SubmitErrorToLog(_errorResponse);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine();
                            Console.WriteLine("That is not a number.");
                            Console.WriteLine("Press ENTER to continue...");
                            Console.ResetColor();
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

            if (_changesCount > 0)
            {
                Console.WriteLine("Order Modifications:");
                Console.WriteLine("----------------------------------------------------------");
            }

            Order orderEditPreview = new Order();

            if (_orderPreEdit.CustomerName != null)
            {
                orderEditPreview.CustomerName = _orderPreEdit.CustomerName;
                Console.WriteLine("-Customer Name: {0} ---> {1}", _orderToEdit.CustomerName, orderEditPreview.CustomerName);
            }
            else
            {
                orderEditPreview.CustomerName = _orderToEdit.CustomerName;
            }

            if (_orderPreEdit.State != null)
            {
                orderEditPreview.State = _orderPreEdit.State;
                orderEditPreview.TaxRate = _orderPreEdit.TaxRate;
                Console.WriteLine("-State: {0} ---> {1}", _orderToEdit.State, orderEditPreview.State);
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
                Console.WriteLine("-Product Type: {0} ---> {1}", _orderToEdit.ProductType, orderEditPreview.ProductType);
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
                Console.WriteLine("-Area: {0} sqft ---> {1} sqft", _orderToEdit.Area, orderEditPreview.Area);
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

            if (_changesCount > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Here is your updated Order Summary: ");
                Console.WriteLine();
            }

            if (_changesCount == 0)
            {
                Console.WriteLine("You didn't make any changes.");
                Console.WriteLine();
                Console.WriteLine("Here is your Order Summary");
                Console.WriteLine();
            }

            Console.WriteLine("Order Number ----- {0}", orderEditPreview.OrderNumber);
            Console.WriteLine("Order Date ------- {0:d}", orderEditPreview.OrderDate);
            Console.WriteLine("Customer Name ---- {0}", orderEditPreview.CustomerName);
            Console.WriteLine("State ------------ {0}", orderEditPreview.State);
            Console.WriteLine("Product Type ----- {0}", orderEditPreview.ProductType);
            Console.WriteLine("Area ------------- {0:N} sqft", orderEditPreview.Area);
            Console.WriteLine("Materials Cost --- {0:c}", orderEditPreview.MaterialCost);
            Console.WriteLine("Labor Cost ------- {0:c}", orderEditPreview.LaborCost);
            Console.WriteLine("Subtotal --------- {0:c}", orderEditPreview.Total - orderEditPreview.Tax);
            Console.WriteLine("Tax -------------- {0:c}", orderEditPreview.Tax);
            Console.WriteLine("Total ------------ {0:c}", orderEditPreview.Total);
            Console.WriteLine();

            if (_changesCount > 0)
            {
                string input = "";
                do
                {
                    Console.Write("Submit updated Order Information? (Y)es, (N)o, or (Q)uit: ");
                    input = Console.ReadLine().ToUpper();

                } while (input != "Y" && input != "N" && input !="Q");

                if (input == "Y")
                {
                    //Run method and updated orderInfo to BLL with orderEditPreview
                    var response = _oops.SubmitEditOrderToRepo(orderEditPreview);
                    Console.WriteLine(response.Message);
                    Console.WriteLine("Press ENTER to return to MAIN MENU.");
                    Console.ReadLine();
                    return true;

                }
                else if (input == "N")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            else
            {
                string input;
                do
                {
                    Console.Write("Edit again? (Y)es or (N)o: ");
                    input = Console.ReadLine().ToUpper();

                } while (input != "Y" && input != "N");

                if (input == "Y")
                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }

        }

    }
}