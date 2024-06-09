using Dapper;
using ModelLayer.Entity;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using StackExchange.Redis;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class AgentCommisionService : IAgentCommisionService
    {
        private readonly DapperContext _context;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabase _cache;

        public AgentCommisionService(DapperContext context, ConnectionMultiplexer redisConnection)
        {
            _context = context;
            _cache = redisConnection.GetDatabase();
        }

        public async Task<decimal> CalculateCommission(int agentId, int policyId, decimal premiumAmount)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("AgentId", agentId, DbType.Int32);
                parameters.Add("PolicyId", policyId, DbType.Int32);
                parameters.Add("PremiumAmount", premiumAmount, DbType.Decimal);

                using (var connection = _context.CreateConnection())
                {
                    // Execute the query to fetch commission amount
                    var commissions = await connection.QueryAsync<decimal>("sp_CalculateCommission", parameters, commandType: CommandType.StoredProcedure);

                    // Check if there are any commission amounts returned
                    if (commissions.Any())
                    {
                        // If there are multiple commission amounts, take the first one
                        var commissionAmount = commissions.First();

                        _logger.Info($"Commission calculated for AgentId: {agentId}, PolicyId: {policyId}, PremiumAmount: {premiumAmount}");

                        return commissionAmount;
                    }
                    else
                    {
                        // No commission amount found
                        return 0; // Or throw an exception as per your business logic
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error calculating commission");
                throw;
            }
        }



        public async Task<IEnumerable<Commision>> ViewCommissions(int agentId)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("AgentId", agentId, DbType.Int32);

                    var commissions = await connection.QueryAsync<Commision>("sp_ViewCommissions", parameters, commandType: CommandType.StoredProcedure);
                    _logger.Info($"Commissions viewed for AgentId: {agentId}");
                    return commissions;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error viewing commissions");
                throw;
            }
        }

        public async Task PayCommission(int agentId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("AgentId", agentId, DbType.Int32);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync("sp_PayAgentCommission", parameters, commandType: CommandType.StoredProcedure);
                    _logger.Info($"Commission paid for AgentId: {agentId}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error paying commission");
                throw;
            }
        }


    }
}
