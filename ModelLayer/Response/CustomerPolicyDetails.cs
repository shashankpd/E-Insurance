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

        public string PolicyName { get; set; }       // Added PolicyName property

        public decimal CoverageAmount { get; set; }
        public int PolicyTerm { get; set; } // Updated to match the alias in the SQL query
        public string PolicyType { get; set; }
        public string PaymentFrequency { get; set; }
        public int PolicyId { get; set; }
        public DateTime CalculationCreatedAt { get; set; }
        public int CustomerAge { get; set; }
        public decimal PremiumAmount { get; set; }
        public string AgentName { get; set; }
        public string AgentLocation { get; set; }
    }
}
