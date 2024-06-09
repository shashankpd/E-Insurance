using ModelLayer.Entity;
using NLog;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class PolicyRenewalService: IRenewal
    {
        private readonly IRenewal _repository;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public PolicyRenewalService(IRenewal repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public CustomerPolicy GetCustomerPolicyById(int customerPolicyId)
        {
            return _repository.GetCustomerPolicyById(customerPolicyId);
        }

        public bool RenewPolicy(int customerPolicyId)
        {
            try
            {
                CustomerPolicy customerPolicy = _repository.GetCustomerPolicyById(customerPolicyId);

                if (customerPolicy != null)
                {
                    // Check if policy is completed
                    if (IsPolicyCompleted(customerPolicy))
                    {
                        // Renew policy
                        return _repository.RenewPolicy(customerPolicyId);
                    }
                    else
                    {
                        // Policy not completed
                        _logger.Warn($"Policy with ID {customerPolicyId} is not yet completed for renewal.");
                    }
                }
                else
                {
                    // Policy not found
                    _logger.Error($"Policy with ID {customerPolicyId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                _logger.Error($"Error occurred while renewing policy with ID {customerPolicyId}: {ex.Message}");
            }

            return false;
        }

        private bool IsPolicyCompleted(CustomerPolicy customerPolicy)
        {
            // Assuming PolicyTerm is in years
            DateTime policyEndDate = customerPolicy.PurchaseDate.AddYears(customerPolicy.PolicyTerm);

            // Check if the policy end date is in the past
            return (policyEndDate < DateTime.Now);
        }
    }
}
