﻿using System;
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
        public OrderOperations _oops;

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
            string input = "";

            Console.Clear();
            Console.Write("Enter your desired Name: ");
            string customerName = Console.ReadLine();
            do
            {
                do
                {
                    Console.Write("Is {0} the correct Name? (Y)es or (N)o: ", customerName);
                    input = Console.ReadLine().ToUpper();
                    if (input != "Y" && input != "N")
                    {
                        Console.WriteLine("That is not a valid input.");
                    }
                } while (input != "Y" && input != "N");
                if (input == "Y")
                {
                    validName = true;
                }
                else
                {
                    Console.Clear();
                    Console.Write("OK. Please enter your new desired Name: ");
                    customerName = Console.ReadLine();
                }

            } while (!validName);

            _orderInfo.CustomerName = customerName;
        }

        public void PromptStateFromUser()
        {
            bool validState = false;
            string stateInput = "";
            var listOfStates = _oops.FetchListOfStates();

            do
            {
                do
                {
                    Console.Clear();
                    //Show list of States and their tax rates
                    Console.WriteLine("List of States and their tax rates:\n");
                    foreach (var s in listOfStates)
                    {
                        Console.WriteLine("{0} ({1}) - {2}%", s.StateAbbreviation, s.StateName, s.TaxRate);
                    }

                    Console.WriteLine();
                    Console.Write("Enter the state abbreviation of where you're making this order (ex. OH for Ohio): ");
                    stateInput = Console.ReadLine();
                    if (stateInput.Length != 2)
                    {
                        _errorResponse.ErrorTime = DateTime.Now;
                        _errorResponse.ErrorSourceMethod = "Add Order Method";
                        _errorResponse.Message = "State input not 2 letters long.";
                        _errorResponse.Input = stateInput;
                        _oops.SubmitErrorToLog(_errorResponse);

                        Console.WriteLine("That is not two letters. Press ENTER to continue.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                } while (stateInput.Length != 2);

                
                var response = _oops.FetchStateTaxInfo(stateInput);

                if (response.Success)
                {
                    _orderInfo.State = response.StateTaxInfo.StateAbbreviation;
                    _orderInfo.TaxRate = response.StateTaxInfo.TaxRate;
                    Console.WriteLine("You have chosen {0}. It has a tax rate of {1}%.",
                        response.StateTaxInfo.StateName, _orderInfo.TaxRate);
                    Console.WriteLine("Press ENTER to continue.");
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

                    Console.WriteLine("Error occured!!");
                    Console.WriteLine(response.Message);
                    Console.ReadLine();
                }

            } while (!validState);
        }

        public void PromptProductTypeFromUser()
        {
            bool validProductType = false;
            string productInput = "";

            _productsList = _oops.FetchProductsList();

            do
            {
                Console.Clear();
                Console.WriteLine("Here is a list of our products, CostPerSquareFoot, and LaborCostPerSquareFoot");

                foreach (var product in _productsList)
                {
                    Console.WriteLine("{0}:", product.ProductType);
                    Console.WriteLine("-Cost/sqft: {0:c}", product.CostPerSquareFoot);
                    Console.WriteLine("-Labor cost/sqft: {0:c}", product.LaborCostPerSquareFoot);
                    Console.WriteLine();
                }

                //fixed empty product entry glitch
                do
                {
                    Console.Write("Enter the product type of what you want to order: ");
                    productInput = Console.ReadLine();

                    if (productInput == "")
                    {
                        _errorResponse.ErrorTime = DateTime.Now;
                        _errorResponse.ErrorSourceMethod = "Add Order Method";
                        _errorResponse.Message = "Product input is blank.";
                        _errorResponse.Input = productInput;
                        _oops.SubmitErrorToLog(_errorResponse);

                        Console.WriteLine("You must enter something. Press ENTER to conintue.");
                        Console.ReadLine();
                    }
                } while (productInput == "");

                string input;

                //Quick check to see if product type is in list before confirming it to the system
                var pString = new Product {ProductType = productInput};
                

                if (_productsList.Contains(pString))
                {
                    do
                    {
                        Console.Write("Is {0} the correct product that you want to order? (Y)es or (N)o: ", productInput);
                        input = Console.ReadLine().ToUpper();
                        if (input != "Y" && input != "N")
                        {
                            Console.WriteLine("That is not a valid input.");
                        }
                    } while (input != "Y" && input != "N");


                    if (input == "Y")
                    {
                        var response = _oops.FetchProductInfo(productInput);

                        if (response.Success)
                        {
                            _orderInfo.ProductType = response.ProductInfo.ProductType;
                            _orderInfo.CostPerSquareFoot = response.ProductInfo.CostPerSquareFoot;
                            _orderInfo.LaborCostPerSquareFoot = response.ProductInfo.LaborCostPerSquareFoot;
                            Console.Clear();
                            Console.WriteLine(
                                "You have selected {0} with a cost/sqft of {1:c} and a labor cost/sqft of {2:c}.",
                                _orderInfo.ProductType, _orderInfo.CostPerSquareFoot,
                                _orderInfo.LaborCostPerSquareFoot);
                            Console.WriteLine("Press ENTER to continue.");
                            Console.ReadLine();
                            validProductType = true;
                        }
                        else
                        {
                            _errorResponse.ErrorTime = DateTime.Now;
                            _errorResponse.ErrorSourceMethod = "Add Order Method";
                            _errorResponse.Message = "Product type is not in the database.";
                            _errorResponse.Input = productInput;
                            _oops.SubmitErrorToLog(_errorResponse);

                            Console.Clear();
                            Console.WriteLine("Error occurred!!");
                            Console.WriteLine(response.Message);
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.Write("OK. We will display the products list again. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    _errorResponse.ErrorTime = DateTime.Now;
                    _errorResponse.ErrorSourceMethod = "Add Order Method";
                    _errorResponse.Message = "Product type is not in the database.";
                    _errorResponse.Input = productInput;
                    _oops.SubmitErrorToLog(_errorResponse);

                    Console.WriteLine("That product is not in the database. Press ENTER to continue.");
                    Console.ReadLine();
                }

            } while (!validProductType);


        }

        public void PromptAreaFromUser()
        {
            bool validArea = false;
            bool validDec = false;
            string areaInputString = "";
            decimal areaInputDecimal;

            do
            {
                Console.Clear();
                do
                {
                    Console.Write("How many sqft of {0} would you like to purchase? ", _orderInfo.ProductType);
                    areaInputString = Console.ReadLine();
                    if (decimal.TryParse(areaInputString, out areaInputDecimal))
                    {
                        if (areaInputDecimal > 0)
                        {
                            validDec = true;
                        }
                        else
                        {
                            _errorResponse.ErrorTime = DateTime.Now;
                            _errorResponse.ErrorSourceMethod = "Add Order Method";
                            _errorResponse.Message = "Area input is not a number greater than 0.";
                            _errorResponse.Input = areaInputString;
                            _oops.SubmitErrorToLog(_errorResponse);

                            Console.WriteLine("You must enter a value greater than 0!! Press ENTER to continue.");
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

                        Console.WriteLine("That is not a number. Press ENTER to continue.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                } while (!validDec);
                string input;
                do
                {
                    Console.Write("Are you sure you want to purchase {0} sqft of {1}? (Y)es or (N)o: ", areaInputDecimal,
                        _orderInfo.ProductType);
                    input = Console.ReadLine().ToUpper();
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
                    else if (input == "N")
                    {
                        Console.WriteLine("Please enter your new desired area. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid input. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                } while (input != "Y" && input != "N");

            } while (!validArea);
        }

        public void DisplayOrderSummary()
        {

            Console.Clear();
            Console.WriteLine("Order Summary:");
            Console.WriteLine();
            Console.WriteLine("Customer Name: {0}", _orderInfo.CustomerName);
            Console.WriteLine("State: {0}", _orderInfo.State);
            Console.WriteLine("Product Type: {0}", _orderInfo.ProductType);
            Console.WriteLine("Area : {0} sqft", _orderInfo.Area);
            Console.WriteLine("Materials Cost: {0:c}", _orderInfo.MaterialCost);
            Console.WriteLine("Labor Cost: {0:c}", _orderInfo.LaborCost);
            Console.WriteLine("Subtotal: {0:c}", _orderInfo.Total - _orderInfo.Tax);
            Console.WriteLine("Tax: {0:c}", _orderInfo.Tax);
            Console.WriteLine("TOTAL COST: {0:c}", _orderInfo.Total);
            Console.WriteLine();

            string input = "";
            do
            {
                Console.Write("Is this order info accurate? (Y)es or (N)o: ");
                input = Console.ReadLine().ToUpper();
                if (input != "Y" && input != "N")
                {
                    Console.WriteLine("Invalid input. Press ENTER to continue.");
                    Console.ReadLine();
                }
            } while (input != "Y" && input != "N");

            if (input == "Y")
            {
                _orderInfo.OrderDate = DateTime.Now;
                //Determine new order number
                //Success for new order with information
                _validOrderInfo = true;
            }
            else
            {
                Console.WriteLine("OK. We will now reenter all your new order information. Press ENTER to continue.");
                Console.ReadLine();
            }
        }

        //public Response SubmitOrder(Order OrderInfo)
        //{
        //    var response = new Response();

        //    return response;
        //}

        public void DisplayOrderSubmitSuccess()
        {
            Console.Clear();
            Console.WriteLine("{0}, you have successfully submited your new order #{1} to our database on {2:d}. Here is your order information below:",
                _orderInfo.CustomerName, _orderInfo.OrderNumber, _orderInfo.OrderDate);
            Console.WriteLine();
            Console.WriteLine("Order Number: {0}", _orderInfo.OrderNumber);
            Console.WriteLine("Order Date: {0:d}", _orderInfo.OrderDate);
            Console.WriteLine("Customer Name: {0}", _orderInfo.CustomerName);
            Console.WriteLine("State: {0}", _orderInfo.State);
            Console.WriteLine("Product Type: {0}", _orderInfo.ProductType);
            Console.WriteLine("Area : {0} sqft", _orderInfo.Area);
            Console.WriteLine("Materials Cost: {0:c}", _orderInfo.MaterialCost);
            Console.WriteLine("Labor Cost: {0:c}", _orderInfo.LaborCost);
            Console.WriteLine("Subtotal: {0:c}", _orderInfo.Total - _orderInfo.Tax);
            Console.WriteLine("Tax: {0:c}", _orderInfo.Tax);
            Console.WriteLine("TOTAL COST: {0:c}", _orderInfo.Total);
            Console.WriteLine();
            Console.WriteLine("Press ENTER to return to the Main Menu.");
            Console.ReadLine();
        }
    }
}
