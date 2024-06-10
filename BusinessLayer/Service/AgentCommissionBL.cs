using BusinessLayer.Interface;
using ModelLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AgentCommissionBL : IAgentCommissionBL
    {
        private readonly IAgentCommisionService AgentCommission;

        public AgentCommissionBL(IAgentCommisionService AgentCommission)
        {
            this.AgentCommission = AgentCommission;
        }

        //start
        public Task<decimal> CalculateCommission(int agentId, int policyId, decimal premiumAmount)
        {
            return AgentCommission.CalculateCommission(agentId, policyId, premiumAmount);
        }

        public async Task<IEnumerable<Commision>> ViewCommissions(int agentId)
        {
            return await AgentCommission.ViewCommissions(agentId);
        }

        public async Task PayCommission(int agentId)
        {
            await AgentCommission.PayCommission(agentId);
        }
    }
}
