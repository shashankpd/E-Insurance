using Dapper;
using ModelLayer.Entity;
using ModelLayer.Response;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class CustomerPolicyPurchaseService : ICustomerPolicyPurchaseService
    {
        private readonly DapperContext _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CustomerPolicyPurchaseService(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> PolicyPurchase(PolicyPurchase policy)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("CustomerId", policy.CustomerId);
                parameters.Add("PolicyId", policy.PolicyId);
                parameters.Add("PurchaseDate", policy.PurchaseDate);
                parameters.Add("AgentId", policy.AgentId);
                parameters.Add("AnnualIncome", policy.AnnualIncome);
                parameters.Add("DateOfBirth", policy.DateOfBirth);
                parameters.Add("FirstName", policy.FirstName);
                parameters.Add("LastName", policy.LastName);
                parameters.Add("Gender", policy.Gender);
                parameters.Add("MobileNumber", policy.MobileNumber);
                parameters.Add("Address", policy.Address);

                var rowsAffected = await _context.CreateConnection().ExecuteAsync("sp_PurchasePolicy", parameters);

                Logger.Info($"Policy purchased successfully: CustomerId={policy.CustomerId}, PolicyId={policy.PolicyId}");

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while purchasing a policy");
                throw;
            }
        }

        public async Task<IEnumerable<CustomerPolicyDetails>> GetCustomerPurchaseDetailsById(int customerId)
        {
            try
            {
                var policies = await _context.CreateConnection().QueryAsync<CustomerPolicyDetails>(
                    "ViewCustomerPolicies",
                    new { CustomerId = customerId },
                    commandType: CommandType.StoredProcedure
                );

                Logger.Info("Retrieved all policies");

                return policies;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while retrieving policies");
                throw;
            }
        }

        public async Task<bool> RemoveCustomerPolicy(int customerPolicyId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("CustomerPolicyId", customerPolicyId);

                var rowsAffected = await _context.CreateConnection().ExecuteAsync(
                    "DeletePolicy",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                Logger.Info($"Policy with CustomerPolicyId={customerPolicyId} deleted successfully");

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"An error occurred while deleting policy with CustomerPolicyId={customerPolicyId}");
                throw;
            }
        }

        public async Task<IEnumerable<AllCustomerPolicyDetails>> GetAllPurchases()
        {
            try
            {
                var policies = await _context.CreateConnection().QueryAsync<AllCustomerPolicyDetails>(
                    "ViewAllCustomerPolicies",
                    commandType: CommandType.StoredProcedure
                );

                Logger.Info("Retrieved all policies");

                return policies;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while retrieving policies");
                throw;
            }
        }




    }
}
