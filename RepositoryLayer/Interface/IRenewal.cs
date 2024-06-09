using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
   public interface IRenewal
    {
        CustomerPolicy GetCustomerPolicyById(int customerPolicyId);
        bool RenewPolicy(int customerPolicyId);
    }
}
