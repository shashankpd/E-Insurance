using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.Response;
using Response;
using System;
using System.Threading.Tasks;

namespace E_Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyCreationController : ControllerBase
    {
        private readonly IPolicyCreationBL _policyCreation;

        public PolicyCreationController(IPolicyCreationBL policyCreation)
        {
            _policyCreation = policyCreation;
        }

        [HttpPost("policy")]
        public async Task<IActionResult> AddPolicy([FromBody] PolicyCreation policy)
        {
            try
            {
                var result = await _policyCreation.AddPolicy(policy);
                if (result != null)
                {
                    var response = new ResponseModel<PolicyCreation>
                    {
                        Success = true,
                        Message = "Policy created successfully",
                       
                    };
                    return CreatedAtAction(nameof(AddPolicy),response);
                }
                else
                {
                    return BadRequest(new ResponseModel<PolicyCreation>
                    {
                        Success = false,
                        Message = "Invalid input or business rule violation"
                    });
                }
            }
            catch (PolicyAlreadyExistsException ex)
            {
                var response = new ResponseModel<PolicyCreation>
                {
                    Success = false,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<PolicyCreation>
                {
                    Success = false,
                    Message = "An error occurred while creating the policy"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPolicy()
        {
            try
            {
                var result = await _policyCreation.GetAllPolicy();
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<PolicyCreationResponse>>
                    {
                        Success = true,
                        Message = "Policies retrieved successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseModel<PolicyCreationResponse>
                    {
                        Success = false,
                        Message = "No policies found"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<PolicyCreationResponse>
                {
                    Success = false,
                    Message = "An error occurred while retrieving policies"
                });
            }
        }


    }
}

