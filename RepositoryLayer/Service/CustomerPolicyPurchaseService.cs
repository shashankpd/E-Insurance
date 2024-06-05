using Dapper;
using ModelLayer.Entity;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
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
    }
}
