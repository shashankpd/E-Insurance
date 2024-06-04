using Dapper;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NLog;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using ModelLayer.RequestDTO;

namespace RepositoryLayer.Service
{
    public class RegistrationService : IRegistrationService
    {
        private readonly DapperContext _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;

        public RegistrationService(DapperContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> AdminRegistration(AdminRegistrationModel adminRegistration)
        {
            try
            {
                string hashedPassword = HashPassword(adminRegistration.PasswordHash);
                var query = "INSERT INTO AdminRegistration (Name, Email, PasswordHash, PhoneNumber, Role,CreatedDate) VALUES (@Name, @Email, @PasswordHash, @PhoneNumber, @Role,@CreatedDate)";

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, new
                    {
                        adminRegistration.Name,
                        adminRegistration.Email,
                        PasswordHash = hashedPassword,
                        adminRegistration.PhoneNumber,
                        adminRegistration.Role,
                        CreatedDate = DateTime.Now
                    });

                    if (affectedRows > 0)
                    {
                        await SendWelcomeEmail(adminRegistration.Email, adminRegistration.Name, adminRegistration.PasswordHash);
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while adding user");
                throw;
            }
        }

        public async Task<bool> CustomerRegistration(CustomerRegistrationModel Customer)
        {
            try
            {
                string hashedPassword = HashPassword(Customer.PasswordHash);
                var query = "INSERT INTO CustomerRegistration (Name, Email, PasswordHash, PhoneNumber, Role,CreatedDate ,Age, Address, AgentId) VALUES (@Name, @Email, @PasswordHash, @PhoneNumber, @Role,@CreatedDate, @Age, @Address, @AgentId)";

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, new
                    {
                        Customer.Name,
                        Customer.Email,
                        PasswordHash = hashedPassword,
                        Customer.PhoneNumber,
                        Customer.Role,
                        CreatedDate = DateTime.Now,
                        Customer.Age,
                        Customer.Address,
                        Customer.AgentId
                    });

                    if (affectedRows > 0)
                    {
                        await SendWelcomeEmail(Customer.Email, Customer.Name, Customer.PasswordHash);
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while adding user");
                throw;
            }
        }

        public async Task<bool> AgentRegistration(InsuranceAgentRegistrationModel Agent)
        {
            try
            {
                string hashedPassword = HashPassword(Agent.PasswordHash);
                var query = "INSERT INTO InsuranceAgentRegistration (Name, Email, PasswordHash, PhoneNumber, Role,CreatedDate) VALUES (@Name, @Email, @PasswordHash, @PhoneNumber, @Role,@CreatedDate)";

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, new
                    {
                        Agent.Name,
                        Agent.Email,
                        PasswordHash = hashedPassword,
                        Agent.PhoneNumber,
                        Agent.Role,
                        CreatedDate = DateTime.Now
                    });

                    if (affectedRows > 0)
                    {
                        await SendWelcomeEmail(Agent.Email, Agent.Name, Agent.PasswordHash);
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while adding user");
                throw;
            }
        }

        public async Task<bool> EmployeeRegistration(EmployeeRegistrationModel Employee)
        {
            try
            {
                string hashedPassword = HashPassword(Employee.PasswordHash);
                var query = "INSERT INTO EmployeeRegistration (Name, Email, PasswordHash, PhoneNumber, Role,CreatedDate) VALUES (@Name, @Email, @PasswordHash, @PhoneNumber, @Role,@CreatedDate)";

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, new
                    {
                        Employee.Name,
                        Employee.Email,
                        PasswordHash = hashedPassword,
                        Employee.PhoneNumber,
                        Employee.Role,
                        CreatedDate = DateTime.Now
                    });

                    if (affectedRows > 0)
                    {
                        await SendWelcomeEmail(Employee.Email, Employee.Name, Employee.PasswordHash);
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while adding user");
                throw;
            }
        }

        public async Task<string> UserLogin(string email, string password, IConfiguration configuration, string role)
        {
            try
            {
                string query = string.Empty;
                switch (role.ToLower())
                {
                    case "admin":
                        query = "SELECT * FROM AdminRegistration WHERE Email=@Email";
                        break;
                    case "customer":
                        query = "SELECT * FROM CustomerRegistration WHERE Email=@Email";
                        break;
                    case "agent":
                        query = "SELECT * FROM InsuranceAgentRegistration WHERE Email=@Email";
                        break;
                    case "employee":
                        query = "SELECT * FROM EmployeeRegistration WHERE Email=@Email";
                        break;
                    default:
                        throw new UnauthorizedAccessException("Invalid role.");
                }

                using (var connection = _context.CreateConnection())
                {
                    var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });

                    if (user != null)
                    {
                        string hashedPassword = HashPassword(password);

                        if (hashedPassword == user.PasswordHash)
                        {
                            var token = GenerateJwtToken(user, role);
                            return token;
                        }
                    }

                    throw new UnauthorizedAccessException("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while attempting login for user with email: {Email}", email);
                throw;
            }
        }

        private string GenerateJwtToken(User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task SendWelcomeEmail(string email, string username, string password)
        {
            MailMessage mailMessage = new MailMessage();
            try
            {
                mailMessage.From = new MailAddress("pdshashank8@outlook.com", "E-Insurance Management");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Welcome to E-Insurance Management";
                mailMessage.Body = $"Dear {username},<br><br>Thank you for registering with E-Insurance Management.<br>Your login details are as follows:<br>Email: {email}<br>Password: {password}<br><br>Please keep this information secure.<br><br>Best regards,<br>E-Insurance Management Team";
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com")
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("pdshashank8@outlook.com", "PDshashank@123")
                };

                await smtpClient.SendMailAsync(mailMessage);
                Logger.Info("Welcome email sent to {Email}", email);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception caught while sending welcome email to {Email}", email);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }
    }
}
