using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringApp.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Order Order { get; set; }
        public List<Order> OrdersList { get; set; }
        public Tax StateTaxInfo { get; set; }
        public Product ProductInfo { get; set; }
    }
}
