using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringApp.Models
{
    public class Product : IEquatable<Product>
    {
        public string ProductType { get; set; }
        public decimal CostPerSquareFoot { get; set; }
        public decimal LaborCostPerSquareFoot { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Product productToCompare = obj as Product;
            if (productToCompare == null)
            {
                return false;
            }
            return this.Equals(productToCompare);
        }

        public bool Equals(Product productToCompare)
        {
            if (productToCompare == null)
            {
                return false;
            }
            else if (this.ProductType.ToUpper() == productToCompare.ProductType.ToUpper())
            {
                return true;
            }
            return false;
        }
    }
}
