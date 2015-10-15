using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringApp.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Input { get; set; }
        public string ErrorSourceMethod { get; set; }
        public DateTime ErrorTime { get; set; }

    }
}
