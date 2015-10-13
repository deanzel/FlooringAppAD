using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringApp.Models
{
    public abstract class Products
    {
        private string ProductType;
        private decimal CostPerSquareFoot;
        private decimal LaborCostPerSquareFoot;

        private decimal MaterialsCostCalculator(decimal area);
    }
}
