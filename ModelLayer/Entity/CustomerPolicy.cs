using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class CustomerPolicy
    {
        public int CustomerPolicyId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int PolicyTerm { get; set; }
        public DateTime RenewalDate { get; set; } // Added RenewalDate property
        public int AgentId { get; set; }
    }
}
