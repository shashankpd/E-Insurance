using ModelLayer.Entity;
using ModelLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IPolicyCreationBL
    {
      public Task<bool> AddPolicy(PolicyCreation Policy);

        public Task<IEnumerable<PolicyCreationResponse>> GetAllPolicy();


    }
}
