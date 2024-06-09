using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.Response;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CustomerPolicyPurchaseBL : ICustomerPolicyPurchaseBL
    {
        private readonly CustomerPolicyPurchaseService Policypurchase;

        public CustomerPolicyPurchaseBL(CustomerPolicyPurchaseService Policypurchase)
        {
            this.Policypurchase = Policypurchase;
        }

        //start

        public Task<bool> PolicyPurchase(PolicyPurchase policy)
        {
            return Policypurchase.PolicyPurchase(policy);
        } 
        public Task<IEnumerable<CustomerPolicyDetails>> GetCustomerPurchaseDetailsById(int customerId)
        {
            return Policypurchase.GetCustomerPurchaseDetailsById(customerId);
        }

        public Task<bool> RemoveCustomerPolicy(int customerPolicyId)
        {
            return Policypurchase.RemoveCustomerPolicy(customerPolicyId);
        }

    }
}
