using ModelLayer.Entity;
using ModelLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICustomerPolicyPurchaseService
    {
        public Task<bool> PolicyPurchase(PolicyPurchase policy);

        public Task<IEnumerable<CustomerPolicyDetails>> GetCustomerPurchaseDetailsById(int customerId);

        public Task<bool> RemoveCustomerPolicy(int customerPolicyId);

        public Task<IEnumerable<CustomerPolicyDetails>> GetAllPurchases();




    }
}
