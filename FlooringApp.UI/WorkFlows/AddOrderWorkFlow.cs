using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;
using FlooringApp.Models;

namespace FlooringApp.UI.WorkFlows
{
    public class AddOrderWorkFlow
    {
        private Order _orderInfo;
        private List<Product> _productsList;
        private bool _validOrderInfo;
        private OrderOperations _oops;

        private ErrorResponse _errorResponse;

        public AddOrderWorkFlow(OrderOperations oops)
        {
            _oops = oops;
            _orderInfo = new Order();
            _productsList = new List<Product>();
            _errorResponse = new ErrorResponse();
        }

        public void Execute()
        {
            do
            {
                PromptNameFromUser();
                PromptStateFromUser();
                PromptProductTypeFromUser();
                PromptAreaFromUser();

                DisplayOrderSummary();
            } while (!_validOrderInfo);

            //method that sends order downstairs

            _oops.SubmitOrderToRepo(_orderInfo);

            DisplayOrderSubmitSuccess();

            //print order summary with new order number after writing it to database

        }

        public void PromptNameFromUser()
        {
            bool validName = false;
            string input;
            string customerName;
            
            do
            {
                do
                {
                    Console.Clear();
                    Console.Write("Enter your desired Name: ");
                    customerName = Console.ReadLine();
                    Console.WriteLine();

                    if (customerName == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You must enter a Name.");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                } while (customerName == ""); 
                do
                {
                    Console.Write("Is {0} the correct Name? (Y)es or (N)o: ", customerName);
                    input = Console.ReadLine().ToUpper();

                } while (input != "Y" && input != "N");

                if (input == "Y")
                {
                    validName = true;
                }

            } while (!validName);

            _orderInfo.CustomerName = customerName;
        }

        public void PromptStateFromUser()
        {
            bool validState = false;
            string stateInput;
            var listOfStates = _oops.FetchListOfStates();

            do
            {
                do
                {
                    Console.Clear(); 
                    Console.WriteLine("List of States that we service:");
                    Console.WriteLine("-------------------------------");

                    foreach (var s in listOfStates)
                    {
                        Console.WriteLine(" {0} - ({1}) - {2}%", s.StateAbbreviation, s.StateName, s.TaxRate);
                    }

                    Console.WriteLine();
                    Console.Write("Enter the State Abbreviation of where you're making this order: ");
                    stateInput = Console.ReadLine();
                    Console.WriteLine();

                    if (stateInput == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You must enter a State Abbreviation.");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                    
                    else if (stateInput.Length != 2)
                    {
                        _errorResponse.ErrorTime = DateTime.Now;
                        _errorResponse.ErrorSourceMethod = "Add Order Method";
                        _errorResponse.Message = "State input not 2 letters long.";
                        _errorResponse.Input = stateInput;
                        _oops.SubmitErrorToLog(_errorResponse);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That is not a state abbreviation.");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                        Console.Clear();
                    }

                } while (stateInput.Length != 2);

                var response = _oops.FetchStateTaxInfo(stateInput);

                if (response.Success)
                {
                    _orderInfo.State = response.StateTaxInfo.StateAbbreviation;
                    _orderInfo.TaxRate = response.StateTaxInfo.TaxRate;
                    Console.WriteLine("You have chosen {0}. It has a Tax Rate of {1}%.",
                        response.StateTaxInfo.StateName, _orderInfo.TaxRate);
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                    validState = true;
                }

                else
                {
                    _errorResponse.ErrorTime = DateTime.Now;
                    _errorResponse.ErrorSourceMethod = "Add Order Method";
                    _errorResponse.Message = "State input is not in the database.";
                    _errorResponse.Input = stateInput;
                    _oops.SubmitErrorToLog(_errorResponse);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(response.Message);
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }

            } while (!validState);
        }

        public void PromptProductTypeFromUser()
        {
            string productInput;
            string yesNoInput = "";

            _productsList = _oops.FetchProductsList();

            do
            {
                Console.Clear();
                Console.WriteLine("Here is a list of our products:");
                Console.WriteLine("-------------------------------");

                foreach (var product in _productsList)
                {
                    Console.WriteLine(" {0}:", product.ProductType);
                    Console.WriteLine(" -Materials Rate: {0:c}/sqft", product.CostPerSquareFoot);
                    Console.WriteLine(" -Labor Rate: {0:c}/sqft", product.LaborCostPerSquareFoot);
                    Console.WriteLine();
                }
                
                Console.WriteLine("-------------------------------");
                Console.WriteLine();
                Console.Write("Enter the Product Type you want to order: ");
                productInput = Console.ReadLine();
                Console.WriteLine();

                if (productInput == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter a Product Type");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }
                else
                {
                    //Quick check to see if product type is in list before confirming it to the system
                    var pChoice = new Product {ProductType = productInput};

                    if (_productsList.Contains(pChoice))
                    {
                        do
                        {
                            Console.Write("Confirm {0}? (Y)es or (N)o: ", productInput.ToUpper());
                            yesNoInput = Console.ReadLine().ToUpper();

                        } while (yesNoInput != "Y" && yesNoInput != "N");

                        if (yesNoInput == "Y")
                        {
                            var response = _oops.FetchProductInfo(productInput);

                            _orderInfo.ProductType = response.ProductInfo.ProductType;
                            _orderInfo.CostPerSquareFoot = response.ProductInfo.CostPerSquareFoot;
                            _orderInfo.LaborCostPerSquareFoot = response.ProductInfo.LaborCostPerSquareFoot;

                        }
                    }
                    else
                    {
                        _errorResponse.ErrorTime = DateTime.Now;
                        _errorResponse.ErrorSourceMethod = "Add Order Method";
                        _errorResponse.Message = "Product type is not in the database.";
                        _errorResponse.Input = productInput;
                        _oops.SubmitErrorToLog(_errorResponse);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That product is not in the database.");
                        Console.WriteLine("Press ENTER to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                }

            } while (yesNoInput != "Y");
        }

        public void PromptAreaFromUser()
        {
            bool validArea = false;
            string input;
            decimal areaInputDecimal;

            do
            {
                    Console.Clear();
                    Console.WriteLine("You have chosen {0}.", _orderInfo.ProductType);
                    Console.WriteLine("-Materials Rate: {0:c}/sqft", _orderInfo.CostPerSquareFoot);
                    Console.WriteLine("-Labor Rate: {0:c}/sqft", _orderInfo.LaborCostPerSquareFoot);
                    Console.WriteLine();
                    Console.Write("How many sqft of {0} would you like to purchase? ", _orderInfo.ProductType);
                    string areaInputString = Console.ReadLine();

                if (areaInputString == "")
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must enter a Product Area.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }
                else
                {

                    if (decimal.TryParse(areaInputString, out areaInputDecimal))
                    {
                        if (areaInputDecimal > 0)
                        {
                            Console.WriteLine();

                            do
                            {
                                Console.Write("Are you sure you want to purchase {0:N} sqft of {1}? (Y)es or (N)o: ",
                                    areaInputDecimal,
                                    _orderInfo.ProductType);
                                input = Console.ReadLine().ToUpper();

                            } while (input != "Y" && input != "N");

                            if (input == "Y")
                            {
                                _orderInfo.Area = areaInputDecimal;
                                _orderInfo.MaterialCost = _orderInfo.Area*_orderInfo.CostPerSquareFoot;
                                _orderInfo.LaborCost = _orderInfo.Area*_orderInfo.LaborCostPerSquareFoot;
                                decimal subtotal = (_orderInfo.MaterialCost + _orderInfo.LaborCost);
                                _orderInfo.Tax = subtotal*(_orderInfo.TaxRate/100);
                                _orderInfo.Total = subtotal + _orderInfo.Tax;
                                validArea = true;
                            }
                        }
                        else
                        {
                            _errorResponse.ErrorTime = DateTime.Now;
                            _errorResponse.ErrorSourceMethod = "Add Order Method";
                            _errorResponse.Message = "Area input is not a number greater than 0.";
                            _errorResponse.Input = areaInputString;
                            _oops.SubmitErrorToLog(_errorResponse);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You must enter a value greater than 0.");
                            Console.WriteLine("Press ENTER to continue...");
                            Console.ResetColor();
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        _errorResponse.ErrorTime = DateTime.Now;
                        _errorResponse.ErrorSourceMethod = "Add Order Method";
                        _errorResponse.Message = "Area input is not a number.";
                        _errorResponse.Input = areaInputString;
                        _oops.SubmitErrorToLog(_errorResponse);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That is not a number.");
                        Console.WriteLine("Press ENTER to continue.");
                        Console.ResetColor();
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            } while (!validArea);
        }

        public void DisplayOrderSummary()
        {
            Console.Clear();
            Console.WriteLine("Order Summary:");
            Console.WriteLine();
            Console.WriteLine("Customer Name: ---- {0}", _orderInfo.CustomerName);
            Console.WriteLine("State: ------------ {0}", _orderInfo.State);
            Console.WriteLine("Product Type: ----- {0}", _orderInfo.ProductType);
            Console.WriteLine("Area : ------------ {0:N} sqft", _orderInfo.Area);
            Console.WriteLine("Materials Cost: --- {0:c}", _orderInfo.MaterialCost);
            Console.WriteLine("Labor Cost: ------- {0:c}", _orderInfo.LaborCost);
            Console.WriteLine("Subtotal: --------- {0:c}", _orderInfo.Total - _orderInfo.Tax);
            Console.WriteLine("Tax: -------------- {0:c}", _orderInfo.Tax);
            Console.WriteLine("Total: ------------ {0:c}", _orderInfo.Total);
            Console.WriteLine();

            string input;
            do
            {
                Console.Write("Is this order info accurate? (Y)es or (N)o: ");
                input = Console.ReadLine().ToUpper();
            } while (input != "Y" && input != "N");

            if (input == "Y")
            {
                _orderInfo.OrderDate = DateTime.Now;
                _validOrderInfo = true;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("OK. We will now reenter all your new order information");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
        }
        
        public void DisplayOrderSubmitSuccess()
        {
            Console.Clear();
            Console.WriteLine("{0}, you have successfully submitted your new order to our database.", _orderInfo.CustomerName);
            Console.WriteLine("Please save your order confirmation for future reference.");
            
            Console.WriteLine();
            Console.WriteLine("Order Number: ----- {0}", _orderInfo.OrderNumber);
            Console.WriteLine("Order Date: ------- {0:d}", _orderInfo.OrderDate);
            Console.WriteLine("Customer Name: ---- {0}", _orderInfo.CustomerName);
            Console.WriteLine("State: ------------ {0}", _orderInfo.State);
            Console.WriteLine("Product Type: ----- {0}", _orderInfo.ProductType);
            Console.WriteLine("Area: ------------- {0:N} sqft", _orderInfo.Area);
            Console.WriteLine("Materials Cost: --- {0:c}", _orderInfo.MaterialCost);
            Console.WriteLine("Labor Cost: ------- {0:c}", _orderInfo.LaborCost);
            Console.WriteLine("Subtotal: --------- {0:c}", _orderInfo.Total - _orderInfo.Tax);
            Console.WriteLine("Tax: -------------- {0:c}", _orderInfo.Tax);
            Console.WriteLine("Total: ------------ {0:c}", _orderInfo.Total);
            Console.WriteLine();
            Console.WriteLine("Press ENTER to return to the Main Menu.");
            Console.ReadLine();
        }
    }
}