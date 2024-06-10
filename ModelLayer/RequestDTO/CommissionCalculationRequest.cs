using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDTO
{
    public class CommissionCalculationRequest
    {
        public int AgentId { get; set; }
        public int PolicyId { get; set; }
        public decimal PremiumAmount { get; set; }
    }
}
