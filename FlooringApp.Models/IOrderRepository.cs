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

        Order GetOrder(Order OrderInfo);

        Order WriteNewOrderToRepo(Order NewOrder);

        Response RemoveOrderFromRepo(Order OrderToRemove);

        Response EditOrderToRepo(Order OrderWithEdits);
    }
}
