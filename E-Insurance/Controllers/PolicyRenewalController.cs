using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessLayer.Interface;
using System;

namespace E_Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyRenewalController : ControllerBase
    {
        private readonly IRenewalBl _policyRenewalBusinessLogic;
        private readonly ILogger<PolicyRenewalController> _logger;

        public PolicyRenewalController(IRenewalBl policyRenewalBusinessLogic, ILogger<PolicyRenewalController> logger)
        {
            _policyRenewalBusinessLogic = policyRenewalBusinessLogic ?? throw new ArgumentNullException(nameof(policyRenewalBusinessLogic));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // API endpoint for renewing a policy
        [HttpPost("renew")]
        public IActionResult RenewPolicy(int customerPolicyId)
        {
            try
            {
                bool success = _policyRenewalBusinessLogic.RenewPolicy(customerPolicyId);

                if (success)
                {
                    return Ok("Policy renewed successfully.");
                }
                else
                {
                    return BadRequest("Policy renewal failed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while renewing policy with ID {customerPolicyId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
