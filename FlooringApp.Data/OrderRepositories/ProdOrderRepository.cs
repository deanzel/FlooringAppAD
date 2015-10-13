using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Models;

namespace FlooringApp.Data.OrderRepositories
{
    public class ProdOrderRepository : IOrderRepository
    {
        public List<Order> GetOrdersFromDate(DateTime orderDate)
        {
            string filePath = @"DataFiles\Prod\Orders_";
            filePath += orderDate.ToString("MMddyyyy") + ".txt";

            List<Order> orders = new List<Order>();

            try
            {
                var reader = File.ReadAllLines(filePath);

                for (int i = 1; i < reader.Length; i++)
                {
                    var columns = reader[i].Split(',');

                    var order = new Order();

                    order.OrderNumber = int.Parse(columns[0]);
                    order.CustomerName = columns[1];
                    order.State = columns[2];
                    order.TaxRate = decimal.Parse(columns[3]);
                    order.ProductType = columns[4];
                    order.Area = decimal.Parse(columns[5]);
                    order.CostPerSquareFoot = decimal.Parse(columns[6]);
                    order.LaborCostPerSquareFoot = decimal.Parse(columns[7]);
                    order.MaterialCost = decimal.Parse(columns[8]);
                    order.LaborCost = decimal.Parse(columns[9]);
                    order.Tax = decimal.Parse(columns[10]);
                    order.Total = decimal.Parse(columns[11]);

                    orders.Add(order);
                }
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Bro, there are no orders on that date. Come on now...");
                Console.ReadLine();
            }
            return orders;
        }

        public Order GetOrder(DateTime OrderDate, int OrderNumber)
        {
            List<Order> orders = GetOrdersFromDate(OrderDate);

            return orders.FirstOrDefault(o => o.OrderNumber == OrderNumber);
        }
    }
}

