using Dapper;
using ModelLayer.Entity;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class PolicyCreationService : IPolicyCreationService
    {
        private readonly DapperContext _context;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public PolicyCreationService(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPolicy(PolicyCreation policy)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("PolicyName", policy.PolicyName);
                parameters.Add("Description", policy.Description);
                parameters.Add("PolicyType", policy.PolicyType);
                parameters.Add("TermLength", policy.TermLength);
                parameters.Add("CoverageAmount", policy.CoverageAmount);
                parameters.Add("Premium", policy.Premium);
                parameters.Add("EntryAge", policy.EntryAge);

                var rowsAffected = await _context.CreateConnection().ExecuteAsync("CreatePolicy", parameters);

                _logger.Info($"Policy created successfully: {policy.PolicyName}");

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while adding a policy");
                throw;
            }
        }

        public async Task<IEnumerable<PolicyCreation>> GetAllPolicy()
        {
            try
            {
                var policies = await _context.CreateConnection().QueryAsync<PolicyCreation>("ViewPolicy");

                _logger.Info($"Retrieved all policies");

                return policies;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving policies");
                throw;
            }
        }
    }
}
