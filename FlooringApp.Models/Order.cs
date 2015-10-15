using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringApp.Models
{
    //inherited IEquatable interface because 
    //.IndexOf uses IEquatable's Equals method for each member of the Enumerable.
    public class Order : IEquatable<Order>
    {
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string State { get; set; }
        public decimal TaxRate { get; set; }
        public string ProductType { get; set; }
        public decimal Area { get; set; }
        public decimal CostPerSquareFoot { get; set; }
        public decimal LaborCostPerSquareFoot { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }

        //implement IEquatable's Equals with an override
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Order objectToTest = obj as Order;
            if (objectToTest == null)
            {
                return false;
            }
            return this.Equals(objectToTest); //if conversion is successful, run Order's Equals
        }

        public bool Equals(Order order)
        {
            if (order == null)
            {
                return false;
            }
            if (this.OrderNumber == order.OrderNumber) //Only order number deterimes if it's the same order.
            {
                return true;
            }
            return false;
        }
    }
}
