using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using ModelLayer.Entity;
using System.Threading.Tasks;
using Response;
using Microsoft.AspNetCore.Http;
using BusinessLayer.Service;

namespace E_Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyPurchaseController : ControllerBase
    {
        private readonly ICustomerPolicyPurchaseBL _policyPurchaseBL;

        public PolicyPurchaseController(ICustomerPolicyPurchaseBL policyPurchaseBL)
        {
            _policyPurchaseBL = policyPurchaseBL;
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchasePolicy([FromBody] PolicyPurchase policyPurchase)
        {
            try
            {
                var result = await _policyPurchaseBL.PolicyPurchase(policyPurchase);
                if (result)
                {
                    var response = new ResponseModel<PolicyPurchase>
                    {
                        Success = true,
                        Message = "Policy purchased successfully",
                        Data = policyPurchase
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseModel<PolicyPurchase>
                    {
                        Success = false,
                        Message = "Failed to purchase policy"
                    });
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<PolicyPurchase>
                {
                    Success = false,
                    Message = "An error occurred while purchasing the policy"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
