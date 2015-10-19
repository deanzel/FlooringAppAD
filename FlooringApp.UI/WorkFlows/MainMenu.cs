using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;

namespace FlooringApp.UI.WorkFlows
{
    public class MainMenu
    {
        private OrderOperations _oops;

        public MainMenu(OrderOperations oops)
        {
            _oops = oops;
        }
        public void Execute()
        {
            string input = "";
            do
            {
                Console.Clear();
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("| WELCOME TO FLOORING ORDERS MANAGEMENT by Andrew & Dean |");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("|                                                        |");
                Console.WriteLine("| 1. Display Orders                                      |");
                Console.WriteLine("| 2. Add an Order                                        |");
                Console.WriteLine("| 3. Edit an Order                                       |");
                Console.WriteLine("| 4. Remove an Order                                     |");
                Console.WriteLine("| Q. to Quit                                             |");
                Console.WriteLine("|                                                        |");
                Console.WriteLine("----------------------------------------------------------");
                Console.Write("Enter choice: ");
               
                input = Console.ReadLine();

                if (input.ToUpper() != "Q")
                {
                    ProccesChoice(input);
                }

            } while (input.ToUpper() != "Q");
        }

        private void ProccesChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    DisplayOrdersWorkFlow dowf = new DisplayOrdersWorkFlow(_oops);
                    dowf.Execute();
                    break;
                case "2":
                    AddOrderWorkFlow aowf = new AddOrderWorkFlow(_oops);
                    aowf.Execute();
                    break;
                case "3":
                    EditOrderWorkFlow eowf = new EditOrderWorkFlow(_oops);
                    eowf.Execute();
                    break;
                case "4":
                    RemoveOrderWorkFlow rowf = new RemoveOrderWorkFlow(_oops);
                    rowf.Execute();
                    break;
                case "":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("You must enter an option.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("{0} is an invalid entry!", choice);
                    Console.WriteLine("Press enter to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                    break;
            }
        }
    }

}
