/*using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
   public  class AgentComissionServiceBl:IAgentComissionBl
    {
        private readonly IAgentComission _agentcommision;
        public AgentComissionServiceBl(IAgentComission agentcommision)
        {
            _agentcommision = agentcommision;

        }

        public Task<IEnumerable<object>> GetAllAgentComission()
        {
            return _agentcommision.GetAllAgentComission();
        }
    }
}
*/