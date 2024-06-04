using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
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
    public class RegistrationBusinessLigic : IRegistrationBusinessLogic
    {
        private readonly IRegistrationService Registartion;

        public RegistrationBusinessLigic(IRegistrationService Registartion)
        {
            this.Registartion = Registartion;
        }

        //start

        public Task<bool> AdminRegistration(AdminRegistrationModel Admin)
        {
            return Registartion.AdminRegistration(Admin);
        }

        public Task<bool> CustomerRegistration(CustomerRegistrationModel Customer)
        {
            return Registartion.CustomerRegistration(Customer);
        }

        public Task<bool> EmployeeRegistration(EmployeeRegistrationModel Employee)
        {
            return Registartion.EmployeeRegistration(Employee);
        }

        public Task<bool> AgentRegistration(InsuranceAgentRegistrationModel Agent)
        {
            return Registartion.AgentRegistration(Agent);
        }

        public Task<string> UserLogin(string email, string password, IConfiguration configuration, string role)
        {
            return Registartion.UserLogin(email, password, configuration,role);
        }

    }
}