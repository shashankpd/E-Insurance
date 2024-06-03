using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
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

        public Task<bool> RegisterUser(User userRegistration)
        {
            return Registartion.RegisterUser(userRegistration);
        }
        public Task<string> UserLogin(string email, string password, IConfiguration configuration)
        {
            return Registartion.UserLogin(email, password, configuration);
        }

    }
}
