using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
  public  class AgentComissionEntity
    {

        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PolicyType { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal CommissionAmount { get; set; }
    }
}
