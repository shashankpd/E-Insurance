using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class ReceiptDetails
    {
        public int PaymentId { get; set; }
        public int CustomerPolicyId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string AgentName { get; set; }
        public string PolicyName { get; set; }
        public decimal CoverageAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}
