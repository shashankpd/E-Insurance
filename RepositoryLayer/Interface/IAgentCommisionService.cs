using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
public interface IAgentCommisionService
    {
        public Task<decimal> CalculateCommission(int agentId, int policyId, decimal premiumAmount);
        public  Task<IEnumerable<Commision>> ViewCommissions(int agentId);

        public Task PayCommission(int agentId);


    }
}
