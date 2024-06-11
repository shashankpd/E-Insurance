
using ModelLayer.Entity;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;

namespace RepositoryLayer.Service
{
    public class PolicyRenewalService : IRenewal
    {
        private readonly DapperContext _context;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public PolicyRenewalService(DapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public CustomerPolicy GetCustomerPolicyById(int customerPolicyId)
        {
            return _context.GetCustomerPolicyById(customerPolicyId);
        }

        public bool RenewPolicy(int customerPolicyId)
        {
            try
            {
                CustomerPolicy customerPolicy = _context.GetCustomerPolicyById(customerPolicyId);

                if (customerPolicy != null)
                {
                    if (IsPolicyCompleted(customerPolicy))
                    {
                        return _context.RenewPolicy(customerPolicyId);
                    }
                    else
                    {
                        _logger.Warn($"Policy with ID {customerPolicyId} is not yet completed for renewal.");
                        return false;
                    }
                }
                else
                {
                    _logger.Error($"Policy with ID {customerPolicyId} not found.");
                    throw new ArgumentException($"Policy with ID {customerPolicyId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while renewing policy with ID {customerPolicyId}");
                return false;
            }
        }

        private bool IsPolicyCompleted(CustomerPolicy customerPolicy)
        {

            DateTime policyEndDate = customerPolicy.PurchaseDate.AddYears(customerPolicy.PolicyTerm);
            return policyEndDate < DateTime.Now;
        }
    }
}
