using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Models;

namespace FlooringApp.Data.ProductsandTaxes
{
    public class TaxRepo
    {
        public List<Tax> GetTaxesList()
        {
            string filePath = @"DataFiles\StateData.txt";

            List<Tax> taxesList = new List<Tax>();

            var reader = File.ReadAllLines(filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var tax = new Tax();

                tax.StateAbbreviation = columns[0];
                tax.StateName = columns[1];
                tax.TaxRate = decimal.Parse(columns[2]);

                taxesList.Add(tax);
            }

            return taxesList;
        }

        public Tax GetStateTaxInfo(string StateAbbreviation)
        {
            var taxesList = GetTaxesList();
            //fixed lower-case state abbreviation input error
            var stateTax = taxesList.FirstOrDefault(s => s.StateAbbreviation.ToUpper() == StateAbbreviation.ToUpper());

            return stateTax;
        }
    }
}
