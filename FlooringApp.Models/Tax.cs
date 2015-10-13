using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringApp.Models
{
    public abstract class Tax
    {
        private string StateAbbreviation;
        private string StateName;
        private decimal TaxRate;

        private decimal TaxCalculator(decimal subtotal);
    }
}
