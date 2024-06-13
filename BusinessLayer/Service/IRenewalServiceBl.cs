using BusinessLayer.Interface;
using ModelLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class IRenewalServiceBl : IRenewalBl
    {
        private readonly IRenewal _renewalRepository;

        public IRenewalServiceBl(IRenewal renewalRepository)
        {
            _renewalRepository = renewalRepository ?? throw new ArgumentNullException(nameof(renewalRepository));
        }

        public CustomerPolicy GetCustomerPolicyById(int customerPolicyId)
        {
            return _renewalRepository.GetCustomerPolicyById(customerPolicyId);
 
        }

        public bool RenewPolicy(int customerPolicyId)
        {
            return _renewalRepository.RenewPolicy(customerPolicyId);
        }
    }
}
