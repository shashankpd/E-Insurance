using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Response
{
    public class CustomerPolicyDetails
    {
        public int CustomerPolicyId { get; set; }
        public string CustomerName { get; set; }
        public int AgentId { get; set; }
        public string PolicyName { get; set; }
        public string Description { get; set; }
        public string PolicyType { get; set; }
        public DateTime PurchaseDate { get; set; }
       // public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public decimal Premium { get; set; }
        public int TermLength { get; set; }
        public int EntryAge { get; set; }
        public string Status { get; set; }
    }
}
