using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using FlooringApp.Models;

namespace FlooringApp.Data.OrderRepositories
{
    public class MockOrderRepository : IOrderRepository
    {
        private static Dictionary<string, List<Order>> _ordersDictionary = new Dictionary<string, List<Order>>();

        private DateTime _currentTime;
        private string _errorLogPath;

        public MockOrderRepository(string initialBuild)
        {
            //Build initial set of order numbers
            string filePath = @"DataFiles\Mock\";
            string filePathHistory = @"DataFiles\Mock\OrderNumbersHistory.txt";

            //Creates Mock folder if not there
            string filePathMockFolder = @"DataFiles\Mock";
            if (!Directory.Exists(filePathMockFolder))
            {
                Directory.CreateDirectory(filePathMockFolder);
            }

            string[] orderNames = Directory.GetFiles(filePath, "Orders_*.txt");

            List<int> orderNumbersHistory = new List<int>();

            //Write each order date's orders to its own dictionary key
            foreach (var orderPath in orderNames)
            {
                //Sample string path = DataFiles\Mock\Orders_06012015.txt

                List<Order> currentOrdersList = new List<Order>();

                string orderDictionaryKey = orderPath.Substring(22, 8);

                var reader = File.ReadAllLines(orderPath);

                for (int i = 1; i < reader.Length; i++)
                {
                    var columns = reader[i].Split(',');
                    int L = columns.Length;

                    var order = new Order();

                    order.OrderNumber = int.Parse(columns[0]); //first assignment

                    for (int j = 1; j < L - 10; j++) //the name assignments
                    {
                        if (j == L - 11) //last one doesnt get comma
                        {
                            order.CustomerName += columns[j];
                        }
                        else //others get a comma
                        {
                            order.CustomerName += (columns[j] + ",");
                        }
                    }
                    //the other assignments
                    order.State = columns[L - 10];
                    order.TaxRate = decimal.Parse(columns[L - 9]);
                    order.ProductType = columns[L - 8];
                    order.Area = decimal.Parse(columns[L - 7]);
                    order.CostPerSquareFoot = decimal.Parse(columns[L - 6]);
                    order.LaborCostPerSquareFoot = decimal.Parse(columns[L - 5]);
                    order.MaterialCost = decimal.Parse(columns[L - 4]);
                    order.LaborCost = decimal.Parse(columns[L - 3]);
                    order.Tax = decimal.Parse(columns[L - 2]);
                    order.Total = decimal.Parse(columns[L - 1]);

                    currentOrdersList.Add(order);
                }

                _ordersDictionary.Add(orderDictionaryKey, currentOrdersList);

                for (int i = 1; i < reader.Length; i++)
                {
                    var columns = reader[i].Split(',');
                    orderNumbersHistory.Add(int.Parse(columns[0]));
                }
            }

            using (var writer = File.CreateText(filePathHistory))
            {
                foreach (int i in orderNumbersHistory)
                {
                    writer.WriteLine(i);
                }
            }


            //Creating Error Log file and folder (if folder is not there)
            string filePathErrorLogFolder = @"DataFiles\Mock\ErrorLogs";
            if (!Directory.Exists(filePathErrorLogFolder))
            {
                Directory.CreateDirectory(filePathErrorLogFolder);
            }

            _currentTime = DateTime.Now;
            string filePathErrorLog = @"DataFiles\Mock\ErrorLogs\ErrorLog_" + _currentTime.ToString("MMddyyyyhhmmss") +
                                      ".txt";

            _errorLogPath = filePathErrorLog;

            using (var writer = File.CreateText(_errorLogPath))
            {
                writer.WriteLine("This is the error log for the session starting at {0:G}.", _currentTime);
                writer.WriteLine();
                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine();
            }
        }

        public List<Order> GetOrdersFromDate(DateTime OrderDate)
        {
        string dictionaryKeyDate = OrderDate.ToString("MMddyyyy");
        List<Order> orders = new List<Order>();

        _ordersDictionary.TryGetValue(dictionaryKeyDate, out orders);
            
        return orders;
            
        }

        public Order GetOrder(Order OrderInfo)
        {
            List<Order> orders = GetOrdersFromDate(OrderInfo.OrderDate);

            Order updatedOrderInfo = new Order();
            updatedOrderInfo = orders.FirstOrDefault(o => o.OrderNumber == OrderInfo.OrderNumber);

            OrderInfo = updatedOrderInfo;

            return OrderInfo;
        }

        public Order WriteNewOrderToRepo(Order NewOrder)
        {
            string dictionaryKeyDate = NewOrder.OrderDate.ToString("MMddyyyy");
            List<Order> orders = new List<Order>();
            List<Order> ordersTemp = new List<Order>();

            _ordersDictionary.TryGetValue(dictionaryKeyDate, out ordersTemp);

            string filePathOrderHistory = @"DataFiles\Mock\OrderNumbersHistory.txt";
            var reader = File.ReadAllLines(filePathOrderHistory);
            int[] readerInts = Array.ConvertAll(reader, int.Parse);

            NewOrder.OrderNumber = readerInts.Max() + 1;
            if (ordersTemp == null)
            {
                orders.Add(NewOrder);
                _ordersDictionary.Add(dictionaryKeyDate, orders);
            }
            else
            {
                ordersTemp.Add(NewOrder);
                _ordersDictionary.Add(dictionaryKeyDate, ordersTemp);
            }
            
            return NewOrder;
        }

        public Response RemoveOrderFromRepo(Order OrderToRemove)
        {
            string dictionaryKeyDate = OrderToRemove.OrderDate.ToString("MMddyyyy");

            List<Order> ordersComplete = new List<Order>();

            _ordersDictionary.TryGetValue(dictionaryKeyDate, out ordersComplete);

            ordersComplete.RemoveAll(o => o.OrderNumber == OrderToRemove.OrderNumber);

            var response = new Response();
            response.Success = true;
            response.Message = "The order was successfully removed!!";

            return response;
        }

        public Response EditOrderToRepo(Order OrderWithEdits)
        {
            string dictionaryKeyDate = OrderWithEdits.OrderDate.ToString("MMddyyyy");

            List<Order> ordersComplete = new List<Order>();

            _ordersDictionary.TryGetValue(dictionaryKeyDate, out ordersComplete);

            ordersComplete.RemoveAll(o => o.OrderNumber == OrderWithEdits.OrderNumber);

            ordersComplete.Add(OrderWithEdits);

            var response = new Response();

            response.Success = true;
            response.Message = "The order was successfully edited!!";

            return response;
        }

        public void WriteErrorToLog(ErrorResponse ErrorInfo)
        {
            using (var writer = File.AppendText(_errorLogPath))
            {
                writer.WriteLine("Time: {0:G}", ErrorInfo.ErrorTime);
                writer.WriteLine("Error Source Method: {0}", ErrorInfo.ErrorSourceMethod);
                writer.WriteLine("Message: {0}", ErrorInfo.Message);
                writer.WriteLine("User Input: {0}", ErrorInfo.Input);
                writer.WriteLine();
            }
        }
    }
}