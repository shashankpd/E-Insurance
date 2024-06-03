using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using Response;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using UserManagement.Controllers;

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

        [HttpPost("SignUp/Admin")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AdminRegistration([FromBody] AdminRegistrationModel user)
        {
            return await RegisterUser(user, "Admin");
        }

        [HttpPost("SignUp/Employee")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> EmployeeRegistration([FromBody] EmployeeRegistrationModel user)
        {
            return await RegisterUser(user, "Employee");
        }

        [HttpPost("SignUp/Customer")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> CustomerRegistration([FromBody] CustomerRegistrationModel user)
        {
            return await RegisterUser(user, "Customer");
        }

        [HttpPost("SignUp/InsuranceAgent")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> InsuranceAgentRegistration([FromBody] InsuranceAgentRegistrationModel user)
        {
            return await RegisterUser(user, "InsuranceAgent");
        }

        private async Task<IActionResult> RegisterUser<T>(T model, string role)
        {
            _logger.LogInformation("Starting registration for role: {Role}", role);

            if (model == null)
            {
                _logger.LogWarning("Received null registration model for role: {Role}", role);
                return BadRequest("Invalid registration data.");
            }

            try
            {
                _logger.LogInformation("Mapping registration model to User entity for role: {Role}", role);
                var userEntity = MapToEntity(model, role);

                if (userEntity == null)
                {
                    _logger.LogWarning("Mapping failed for role: {Role}", role);
                    return BadRequest("Invalid registration data.");
                }

                _logger.LogInformation("Attempting to register user for role: {Role}", role);
                var result = await _user.RegisterUser(userEntity);

                if (result)
                {
                    _logger.LogInformation("User registration successful for role: {Role}", role);
                    var response = new ResponseModel<T>
                    {
                        Success = true,
                        Message = "User Registration Successful"
                    };
                    return Ok(response);
                }
                else
                {
                    _logger.LogError("User registration failed for role: {Role}", role);
                    return BadRequest("User registration failed. Unknown reason.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration for role: {Role}", role);

                if (ex.Message.Contains("Email address is already in use"))
                {
                    return BadRequest("Email address is already in use.");
                }
                else if (ex is InvalidDataException)
                {
                    return BadRequest("Invalid data provided for registration.");
                }
                else
                {
                    return BadRequest("User registration failed due to an unexpected error.");
                }
            }
        }

        private User MapToEntity<T>(T model, string role)
        {
            _logger.LogInformation("Mapping model to User entity for role: {Role}", role);

            switch (model)
            {
                case AdminRegistrationModel adminModel:
                    return new User
                    {
                        Username = adminModel.Username,
                        Email = adminModel.Email,
                        PasswordHash = adminModel.PasswordHash,
                        PhoneNumber = adminModel.PhoneNumber,
                        CreatedDate = DateTime.UtcNow,
                        Role = role
                    };

                case EmployeeRegistrationModel employeeModel:
                    return new User
                    {
                        Username = employeeModel.FirstName,
                        Email = employeeModel.Email,
                        PasswordHash = employeeModel.PasswordHash,
                        PhoneNumber = employeeModel.PhoneNumber,
                        CreatedDate = DateTime.UtcNow,
                        Role = role
                    };

                case CustomerRegistrationModel customerModel:
                    return new User
                    {
                        Username = customerModel.FirstName,
                        Email = customerModel.Email,
                        PasswordHash = customerModel.PasswordHash,
                        PhoneNumber = customerModel.PhoneNumber,
                        DateOfBirth = customerModel.DateOfBirth,
                        Address = customerModel.Address,
                        CreatedDate = DateTime.UtcNow,
                        Role = role
                    };

                case InsuranceAgentRegistrationModel agentModel:
                    return new User
                    {
                        Username = agentModel.FirstName,
                        Email = agentModel.Email,
                        PasswordHash = agentModel.PasswordHash,
                        PhoneNumber = agentModel.PhoneNumber,
                        CreatedDate = DateTime.UtcNow,
                        Role = role
                    };

                default:
                    _logger.LogWarning("Invalid model type provided for role: {Role}", role);
                    return null;
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(string email, string password, [FromServices] IConfiguration configuration)
        {
            try
            {
                _logger.LogInformation("Starting login process for email: {Email}", email);
                var details = await _user.UserLogin(email, password, configuration);

                var response = new ResponseModel<string>
                {
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
                    var response = new ResponseModel<User>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return Conflict(response);
                }
                else if (ex is InvalidPasswordException)
                {
                    var response = new ResponseModel<User>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
