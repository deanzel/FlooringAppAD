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
         
        public void Execute()
        {
            
            //prompt for order info
        }
        
        //bool validProductType = false;
        //bool validArea = false;

        public void PromptOrderInfoFromUser()
        {
            PromptNameFromUser();
            PromptStateFromUser();
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
                    Console.Write("Is {0} the correct Name? (Y)es or (N)o:", customerName);
                    input = Console.ReadLine().ToUpper();
                    if (input != "Y" || input != "N")
                    {
                        Console.WriteLine("That is not a valid input.");
                    }
                } while (input != "Y" || input != "N");
                if (input == "Y")
                {
                    validName = true;
                }
                else
                {
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

            Console.Clear();

            do
            {
                do
                {
                    Console.Write("Enter the state abbreviation of where you're making this order (ex. TX for Texas): ");
                    stateInput = Console.ReadLine();
                    if (stateInput.Length != 2)
                    {
                        Console.WriteLine("That is not two letters. Press ENTER to continue.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                } while (stateInput.Length != 2);

                var orderOperations = new OrderOperations();
                var response = orderOperations.FetchStateTaxInfo(stateInput);

                if (response.Success == true)
                {
                    _orderInfo.State = response.StateTaxInfo.StateAbbreviation;
                    _orderInfo.TaxRate = response.StateTaxInfo.TaxRate;
                    Console.WriteLine("You have chosen {0}. It has a tax rate of {1:p}.",
                        response.StateTaxInfo.StateName, _orderInfo.TaxRate);
                    validState = true;
                }
                else
                {
                    Console.WriteLine("Error occured!!");
                    Console.WriteLine(response.Message);
                    Console.ReadLine();
                }

            } while (!validState);
        }
    }
}
