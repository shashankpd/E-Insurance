using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICustomerPolicyPurchaseBL
    {
        public Task<bool> PolicyPurchase(PolicyPurchase policy);
    }
}
