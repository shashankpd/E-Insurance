using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.Response;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CustomerPolicyPurchaseBL : ICustomerPolicyPurchaseBL
    {
        private readonly ICustomerPolicyPurchaseService Policypurchase;

        public CustomerPolicyPurchaseBL(ICustomerPolicyPurchaseService Policypurchase)
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
