using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.Data.SqlClient;

public class AgentCommissionService : IAgentCommission
{
    private readonly DapperContext _context;
    private readonly ILogger _logger;

    public AgentCommissionService(DapperContext context, ILogger<AgentCommissionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<object>> GetAllAgentComission()
    {
        var query = @"
            SELECT A.AgentId,
                   A.Name as AgentName,
                   C.CustomerId,
                   C.Nane as CustomerName,
                   P.PolicyType,
                   Pt.Amount AS PremiumAmount,
                   cr.CommissionRate,
                   Pt.Amount * cr.CommissionRate AS CommissionAmount
            FROM InsuranceAgentRegistration AS A


                A.AgentId = C.AgentId
            INNER JOIN Payments AS Pt ON C.CustomerId = Pt.CustomerId
            INNER JOIN ComissionRates AS cr ON A.PolicyType = cr.PolicyType";

        using (var connection = _context.CreateConnection())
        {
            var agentCommissions = await connection.QueryAsync(query);

            if (agentCommissions.Any())
            {
                return agentCommissions;
            }
            else
            {
                throw new Exception("No items are present in the agent commissions.");
            }
        }
    }
}
