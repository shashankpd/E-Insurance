using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using Response;

namespace E_Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class agentcommissionController : ControllerBase
    {
        private readonly IAgentCommissionBL AgentCommission;
        private readonly ILogger<agentcommissionController> _logger; // Add this

        public agentcommissionController(IAgentCommissionBL agentCommission, ILogger<agentcommissionController> logger)
        {
            AgentCommission = agentCommission;
            _logger = logger;
        }

        //start
        [HttpPost("commission")]
        public async Task<IActionResult> CalculateCommission([FromBody] CommissionCalculationRequest request)
        {
            try
            {
                decimal commissionAmount = await AgentCommission.CalculateCommission(request.AgentId, request.PolicyId, request.PremiumAmount);
                if (commissionAmount >= 0)
                {
                    var response = new ResponseModel<decimal>
                    {
                        Success = true,
                        Message = "Commission for Agent calculated successfully",
                        Data = commissionAmount // Set the commission amount as data
                    };
                    return CreatedAtAction(nameof(CalculateCommission), response);
                }
                else
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid input or business rule violation"
                    });
                }
            }
            catch (PolicyAlreadyExistsException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Commission Calculation");

                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the Commission Calculation"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [Authorize]
        [HttpGet("agentid")]
        public async Task<IActionResult> ViewCommissions()
        {
            try
            {
                // Get the AgentId of the authenticated user from the claims in the JWT token
                var agentIdClaim = User.FindFirst("AgentId");

                if (agentIdClaim == null)
                {
                    // Handle case where AgentId claim is missing
                    return Unauthorized("AgentId claim is missing in the token.");
                }

                // Convert authenticated AgentId to int if necessary
                int authenticatedAgentId = int.Parse(agentIdClaim.Value);

                // Proceed with retrieving the commissions for the authenticated agent
                var result = await AgentCommission.ViewCommissions(authenticatedAgentId);
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<Commision>>
                    {
                        Success = true,
                        Message = "All commissions retrieved successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new ResponseModel<IEnumerable<Commision>>
                    {
                        Success = false,
                        Message = "No commissions found",
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving commissions");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving commissions",
                   
                });
            }
        }

        [HttpPost("pay/{agentId}")]
        public async Task<IActionResult> PayCommission(int agentId)
        {
            try
            {
                await AgentCommission.PayCommission(agentId);
                _logger.LogInformation($"Commission paid for AgentId: {agentId}");

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Commission paid for AgentId: {agentId}",
                    Data = "sucess"
                };  
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error paying commission");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while paying commission"
                });
            }
        }

    }    
}
