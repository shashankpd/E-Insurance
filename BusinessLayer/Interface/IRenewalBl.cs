/*using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IRenewalBl
    {
        CustomerPolicy GetCustomerPolicyById(int customerPolicyId);
        bool RenewPolicy(int customerPolicyId);
    }
}
*/




using ModelLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IRenewalBl
    {
       public  bool RenewPolicy(int customerPolicyId);
       public  CustomerPolicy GetCustomerPolicyById(int customerPolicyId);
    }
}
