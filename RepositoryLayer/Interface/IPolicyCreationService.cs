using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IPolicyCreationService
    {
       public Task<bool> AddPolicy(PolicyCreation Policy);

       public Task<IEnumerable<PolicyCreation>> GetAllPolicy();

    }
}
