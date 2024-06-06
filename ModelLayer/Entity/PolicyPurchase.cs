using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class PolicyPurchase
    {
        public int CustomerId { get; set; }
        public int PolicyId { get; set; }
        public DateTime PurchaseDate { get; set; }

        public int AgentId { get; set; }
    }
}
