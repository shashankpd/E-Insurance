using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
<<<<<<< HEAD
public interface IAgentCommisionService
    {
        public Task<decimal> CalculateCommission(int agentId, int policyId, decimal premiumAmount);
        public  Task<IEnumerable<Commision>> ViewCommissions(int agentId);

        public Task PayCommission(int agentId);


=======
    public interface IAgentCommisionService
    {

       public Task<decimal> CalculateCommission(int agentId, int policyId, decimal premiumAmount);
        public Task<IEnumerable<Commision>> ViewCommissions(int agentId);
       public Task PayCommission(int agentId);
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
    }
}
