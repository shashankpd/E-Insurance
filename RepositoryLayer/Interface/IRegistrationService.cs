using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IRegistrationService
    {
        public Task<bool> RegisterUser(User userRegistration);

        public Task<string> UserLogin(string email, string password, IConfiguration configuration);

    }
}
