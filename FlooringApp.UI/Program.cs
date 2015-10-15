using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.BLL;
using FlooringApp.UI.WorkFlows;

namespace FlooringApp.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderOperations oops = new OrderOperations();
            var menu = new MainMenu(oops);
            menu.Execute();
        }
    }
}
