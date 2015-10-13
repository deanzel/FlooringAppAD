using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringApp.Models
{
    public interface IOrderRepository
    {
        List<Order> GetOrdersFromDate(DateTime OrderDate);

        Order GetOrder(DateTime OrderDate, int OrderNumber);
    }
}
