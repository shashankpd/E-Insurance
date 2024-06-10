using ModelLayer.Entity;
using ModelLayer.Response;

namespace BusinessLayer.Interface
{
    public interface IPolicyCreationBL
    {
      public Task<bool> AddPolicy(PolicyCreation Policy);

        public Task<IEnumerable<PolicyCreationResponse>> GetAllPolicy();


    }
}
