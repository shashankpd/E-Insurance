using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using Response;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using UserManagement.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace E_Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IRegistrationBusinessLogic _user;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IRegistrationBusinessLogic user, ILogger<UserManagementController> logger)
        {
            _user = user;
            _logger = logger;
        }

        [HttpPost("AdminRegistration")]
        public async Task<IActionResult> AdminRegistration([FromBody] AdminRegistrationModel admin)
        {
            try
            {
                var details = await _user.AdminRegistration(admin);
                if (details)
                {
                    var response = new ResponseModel<AdminRegistrationModel>
                    {
                        Success = true,
                        Message = "User Registration Successful"
                    };
                    return CreatedAtAction(nameof(AdminRegistration), response);
                }
                else
                {
                    return BadRequest(new ResponseModel<AdminRegistrationModel>
                    {
                        Success = false,
                        Message = "Invalid input"
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex is DuplicateEmailException || ex is InvalidEmailFormatException)
                {
                    var response = new ResponseModel<AdminRegistrationModel>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else
                {
                    _logger.LogError(ex, "An error occurred while adding the user");
                    var response = new ResponseModel<AdminRegistrationModel>
                    {
                        Success = false,
                        Message = "An error occurred while adding the user"
                    };
                    return BadRequest(response);
                }
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("CustomerRegistration")]
        public async Task<IActionResult> CustomerRegistration([FromBody] CustomerRegistrationModel Customer)
        {
            try
            {
                var details = await _user.CustomerRegistration(Customer);
                if (details)
                {
                    var response = new ResponseModel<AdminRegistrationModel>
                    {
                        Success = true,
                        Message = "User Registration Successful"
                    };
                    return CreatedAtAction(nameof(CustomerRegistration), response);
                }
                else
                {
                    return BadRequest(new ResponseModel<CustomerRegistrationModel>
                    {
                        Success = false,
                        Message = "Invalid input"
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex is DuplicateEmailException || ex is InvalidEmailFormatException)
                {
                    var response = new ResponseModel<CustomerRegistrationModel>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else
                {
                    _logger.LogError(ex, "An error occurred while adding the user");
                    var response = new ResponseModel<CustomerRegistrationModel>
                    {
                        Success = false,
                        Message = "An error occurred while adding the user"
                    };
                    return BadRequest(response);
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("AgentRegistration")]
        public async Task<IActionResult> AgentRegistration([FromBody] InsuranceAgentRegistrationModel Agent)
        {
            try
            {
                var details = await _user.AgentRegistration(Agent);
                if (details)
                {
                    var response = new ResponseModel<InsuranceAgentRegistrationModel>
                    {
                        Success = true,
                        Message = "User Registration Successful"
                    };
                    return CreatedAtAction(nameof(AgentRegistration), response);
                }
                else
                {
                    return BadRequest(new ResponseModel<InsuranceAgentRegistrationModel>
                    {
                        Success = false,
                        Message = "Invalid user input"
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex is DuplicateEmailException || ex is InvalidEmailFormatException)
                {
                    var response = new ResponseModel<InsuranceAgentRegistrationModel>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else
                {
                    _logger.LogError(ex, "An error occurred while adding the user");
                    var response = new ResponseModel<InsuranceAgentRegistrationModel>
                    {
                        Success = false,
                        Message = "An error occurred while adding the user"
                    };
                    return BadRequest(response);
                }
            }
        }

        [HttpPost("EmployeeRegistration")]
        public async Task<IActionResult> EmployeeRegistration([FromBody] EmployeeRegistrationModel Employee)
        {
            try
            {
                var details = await _user.EmployeeRegistration(Employee);
                if (details)
                {
                    var response = new ResponseModel<EmployeeRegistrationModel>
                    {
                        Success = true,
                        Message = "User Registration Successful"
                    };
                    return CreatedAtAction(nameof(EmployeeRegistration), response);
                }
                else
                {
                    return BadRequest(new ResponseModel<EmployeeRegistrationModel>
                    {
                        Success = false,
                        Message = "Invalid input"
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex is DuplicateEmailException || ex is InvalidEmailFormatException)
                {
                    var response = new ResponseModel<EmployeeRegistrationModel>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else
                {
                    _logger.LogError(ex, "An error occurred while adding the user");
                    var response = new ResponseModel<EmployeeRegistrationModel>
                    {
                        Success = false,
                        Message = "An error occurred while adding the user"
                    };
                    return BadRequest(response);
                }
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(string email, string password, [FromServices] IConfiguration configuration, string role)
        {
            try
            {
                _logger.LogInformation("Starting login process for email: {Email}", email);
                var details = await _user.UserLogin(email, password, configuration, role);

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Login Successful",
                    Data = details
                };
                _logger.LogInformation("Login successful for email: {Email}", email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login for email: {Email}", email);

                if (ex is NotFoundException)
                {
                    var response = new ResponseModel<string>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return Conflict(response);
                }
                else if (ex is InvalidPasswordException)
                {
                    var response = new ResponseModel<string>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else
                {
                    var response = new ResponseModel<string>
                    {
                        Success = false,
                        Message = "An error occurred during login"
                    };
                    return BadRequest(response);
                }
            }
        }
    }
}


