using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Models;

namespace FlooringApp.Data.OrderRepositories
{
    public class MockOrderRepository : IOrderRepository
    {
        public MockOrderRepository(string initialBuild)
        {
            if (initialBuild.ToUpper() == "Y")
            {
                //Build initial set of order numbers
                string filePath = @"DataFiles\Mock\";
                string filePathHistory = @"DataFiles\Mock\OrderNumbersHistory.txt";

                string[] orderNames = Directory.GetFiles(filePath, "Orders_*.txt");

                List<int> orderNumbersHistory = new List<int>();

                foreach (var orderPath in orderNames)
                {
                    var reader = File.ReadAllLines(orderPath);

                    for (int i = 1; i < reader.Length; i++)
                    {
                        var columns = reader[i].Split(',');
                        int x = int.Parse(columns[0]);
                        orderNumbersHistory.Add(x);
                    }
                }

                using (var writer = File.CreateText(filePathHistory))
                {
                    foreach (int i in orderNumbersHistory)
                    {
                        writer.WriteLine(i);
                    }
                }
            }
        }

        public List<Order> GetOrdersFromDate(DateTime OrderDate)
        {
            string filePath = @"DataFiles\Mock\Orders_";
            filePath += OrderDate.ToString("MMddyyyy") + ".txt";

            List<Order> orders = new List<Order>();
            if (File.Exists(filePath))
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
            return orders;
        }

        public Order GetOrder(DateTime OrderDate, int OrderNumber)
        {
            List<Order> orders = GetOrdersFromDate(OrderDate);

            return orders.FirstOrDefault(o => o.OrderNumber == OrderNumber);
        }

        public Order WriteNewOrderToRepo(Order NewOrder)
        {
            string filePath = @"DataFiles\Mock\Orders_";
            filePath += NewOrder.NewOrderDate.ToString("MMddyyyy") + ".txt";

            //determine new order number
            string filePathOrderHistory = @"DataFiles\Mock\OrderNumbersHistory.txt";
            var reader = File.ReadAllLines(filePathOrderHistory);
            int[] readerInts = Array.ConvertAll(reader, int.Parse);
           
            NewOrder.OrderNumber = readerInts.Max() + 1;

            if (File.Exists(filePath))
            {
                using (var writer = File.AppendText(filePath))
                {
                    writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", NewOrder.OrderNumber,
                        NewOrder.CustomerName,
                        NewOrder.State, NewOrder.TaxRate, NewOrder.ProductType, NewOrder.Area,
                        NewOrder.CostPerSquareFoot, NewOrder.LaborCostPerSquareFoot,
                        NewOrder.MaterialCost, NewOrder.LaborCost, NewOrder.Tax, NewOrder.Total);
                }
            }
            else
            {
                using (var writer = File.CreateText(filePath))
                {
                    writer.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                    writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", NewOrder.OrderNumber,
                        NewOrder.CustomerName,
                        NewOrder.State, NewOrder.TaxRate, NewOrder.ProductType, NewOrder.Area,
                        NewOrder.CostPerSquareFoot, NewOrder.LaborCostPerSquareFoot,
                        NewOrder.MaterialCost, NewOrder.LaborCost, NewOrder.Tax, NewOrder.Total);
                }
            }
            using (var writer = File.AppendText(filePathOrderHistory))
            {
                writer.WriteLine(NewOrder.OrderNumber);
            }

            return NewOrder;
        }
    }
}