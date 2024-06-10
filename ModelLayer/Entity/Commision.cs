using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class Commision
    {
        public int CommissionId { get; set; }
        public int AgentId { get; set; }
        public int PolicyId { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal CommissionAmount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}