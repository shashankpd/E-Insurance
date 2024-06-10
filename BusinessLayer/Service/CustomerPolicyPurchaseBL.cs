using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.Response;
using RepositoryLayer.Interface;
<<<<<<< HEAD
using RepositoryLayer.Service;
=======
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
    
=======

>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
namespace BusinessLayer.Service
{
    public class CustomerPolicyPurchaseBL : ICustomerPolicyPurchaseBL
    {
        private readonly ICustomerPolicyPurchaseService Policypurchase;

<<<<<<< HEAD
        public CustomerPolicyPurchaseBL(ICustomerPolicyPurchaseService Policy)
        {
            Policypurchase = Policy;        
        }       
=======
        public CustomerPolicyPurchaseBL(ICustomerPolicyPurchaseService Policypurchase)
        {
            this.Policypurchase = Policypurchase;
        }

        //start

>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
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
