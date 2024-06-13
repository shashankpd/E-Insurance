using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDTO
{
    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public int CustomerPolicyId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }

        public string Status { get; set; }
    }
}
