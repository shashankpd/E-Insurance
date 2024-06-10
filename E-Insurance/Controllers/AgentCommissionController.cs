using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using Response;

namespace E_Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentCommissionController : ControllerBase
    {
        private readonly IAgentCommissionBL AgentCommission;
        private readonly ILogger<AgentCommissionController> _logger; // Add this

        public AgentCommissionController(IAgentCommissionBL agentCommission, ILogger<AgentCommissionController> logger)
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

        [HttpGet("agentid")]
        public async Task<IActionResult> ViewCommissions(int agentId)
        {
            try
            {
                var result = await AgentCommission.ViewCommissions(agentId);
<<<<<<< HEAD
                if (result != null && result.Any())
=======
                if (result != null)
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
                {
                    var response = new ResponseModel<IEnumerable<Commision>>
                    {
                        Success = true,
                        Message = "All Commissions retrieved successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
<<<<<<< HEAD
                    return BadRequest(new ResponseModel<IEnumerable<Commision>>
                    {
                        Success = false,
                        Message = "No Commissions found",
                        Data = null // or an empty list depending on how you want to handle this case
=======
                    return BadRequest(new ResponseModel<Commision>
                    {
                        Success = false,
                        Message = "No Commissions found"
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Commission");
<<<<<<< HEAD
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<IEnumerable<Commision>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving Commission",
                    Data = null // or an empty list depending on how you want to handle this case
=======
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<Commision>
                {
                    Success = false,
                    Message = "An error occurred while retrieving Commission"
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
                });
            }
        }

<<<<<<< HEAD

=======
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
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
                    Data = null
<<<<<<< HEAD
                };
=======
                };  
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
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

<<<<<<< HEAD
    }
}

=======
    }    
}
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
