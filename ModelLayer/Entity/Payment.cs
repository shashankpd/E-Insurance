using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class Payment
    {
        public int CustomerPolicyId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
      // public string Status { get; set; }
    }
}
