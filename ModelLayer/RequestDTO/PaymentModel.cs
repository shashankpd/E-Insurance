using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDTO
{
    public class PaymentModel
    {
        public int CustomerPolicyId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
<<<<<<< HEAD
=======

        public string Status { get; set; }
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
    }
}
