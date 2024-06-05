using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class PolicyCreationBL : IPolicyCreationBL
    {
        private readonly IPolicyCreationService PolicyCreation;

        public PolicyCreationBL(IPolicyCreationService PolicyCreation)
        {
            this.PolicyCreation = PolicyCreation;
        }

        //start

        public Task<bool> AddPolicy(PolicyCreation Policy)
        {
            return PolicyCreation.AddPolicy(Policy);
        }
        public Task<IEnumerable<PolicyCreation>> GetAllPolicy()
        {
            return PolicyCreation.GetAllPolicy();
        }

    }
}
