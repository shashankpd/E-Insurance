using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IRegistrationService
    {
        public Task<bool> AdminRegistration(AdminRegistrationModel Admin);
        public Task<bool> CustomerRegistration(CustomerRegistrationModel Customer);
        public Task<bool> AgentRegistration(InsuranceAgentRegistrationModel Agent);
        public Task<bool> EmployeeRegistration(EmployeeRegistrationModel Employee);
       




        public Task<string> UserLogin(string email, string password, IConfiguration configuration,string Role);

    }
}
