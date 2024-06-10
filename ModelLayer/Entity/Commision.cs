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
<<<<<<< HEAD
}
=======
}
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
