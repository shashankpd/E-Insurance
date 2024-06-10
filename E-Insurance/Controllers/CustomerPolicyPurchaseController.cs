using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using ModelLayer.Entity;
using System.Threading.Tasks;
using Response;
using Microsoft.AspNetCore.Http;
using BusinessLayer.Service;
using ModelLayer.Response;

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

        [HttpGet]
        public async Task<IActionResult> GetCustomerPurchaseDetailsById(int customerId)
        {
            try
            {
                var result = await _policyPurchaseBL.GetCustomerPurchaseDetailsById(customerId);
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<CustomerPolicyDetails>>
                    {
                        Success = true,
                        Message = "Customer details retrieved successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseModel<IEnumerable<CustomerPolicyDetails>>
                    {
                        Success = false,
                        Message = "No Details found"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<CustomerPolicyDetails>
                {
                    Success = false,
                    Message = "An error occurred while retrieving Customer Details"
                });
            }
        }
        [HttpDelete("{customerPolicyId}")]
        public async Task<IActionResult> RemoveCustomerPolicy(int customerPolicyId)
        {
            try
            {
                var result = await _policyPurchaseBL.RemoveCustomerPolicy(customerPolicyId);
                if (result)
                {
                    return Ok(new ResponseModel<bool>
                    {
                        Success = true,
                        Message = $"Policy with CustomerPolicyId={customerPolicyId} has been successfully removed",
                        Data = true
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = $"Failed to remove policy with CustomerPolicyId={customerPolicyId}",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<bool>
                {
                    Success = false,
                    Message = $"An error occurred while removing policy with CustomerPolicyId={customerPolicyId}",
                    Data = false
                });
            }
        }
    }
}
