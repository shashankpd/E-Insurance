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

        public async Task<bool> RegisterUser(User user)
        {
            var parametersToCheckEmailIsValid = new DynamicParameters();
            parametersToCheckEmailIsValid.Add("Email", user.Email, DbType.String);
            var querytoCheckEmailIsNotDuplicate = @"SELECT COUNT(*) FROM Users WHERE Email = @Email;";

            // Hash the password before saving it to the database
            user.PasswordHash = HashPassword(user.PasswordHash);

            var userQuery = @"INSERT INTO Users(Username, Email, PasswordHash, PhoneNumber, Role, CreatedDate) 
                      VALUES (@Username, @Email, @PasswordHash, @PhoneNumber, @Role, @CreatedDate);";
            var parameters = new DynamicParameters();
            parameters.Add("Username", user.Username, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("PasswordHash", user.PasswordHash, DbType.String);
            parameters.Add("PhoneNumber", user.PhoneNumber, DbType.String);
            parameters.Add("Role", user.Role, DbType.String);
            parameters.Add("CreatedDate", user.CreatedDate, DbType.DateTime);

            using (var connection = _context.CreateConnection())
            {
                // Check if email already exists
                bool emailExists = await connection.QueryFirstOrDefaultAsync<bool>(querytoCheckEmailIsNotDuplicate, parametersToCheckEmailIsValid);
                if (emailExists)
                {
                    throw new Exception("Email address is already in use");
                }

                // Insert new user into Users table
                var userResult = await connection.ExecuteAsync(userQuery, parameters);

                if (userResult > 0)
                {
                    switch (user.Role)
                    {
                        case "Admin":
                            var adminQuery = @"INSERT INTO AdminRegistration (Username, Email, PasswordHash, PhoneNumber, CreatedDate) 
                                       VALUES (@Username, @Email, @PasswordHash, @PhoneNumber, @CreatedDate);";
                            await connection.ExecuteAsync(adminQuery, parameters);
                            break;
                        case "Employee":
                            var employeeQuery = @"INSERT INTO EmployeeRegistration (FirstName, Email, PasswordHash, PhoneNumber, CreatedDate) 
                                          VALUES (@Username, @Email, @PasswordHash, @PhoneNumber, @CreatedDate);"; // Assuming Username is FirstName for Employee
                            await connection.ExecuteAsync(employeeQuery, parameters);
                            break;
                        case "Customer":
                            var customerQuery = @"INSERT INTO CustomerRegistration (FirstName, Email, PasswordHash, PhoneNumber, DateOfBirth, Address) 
                                          VALUES (@Username, @Email, @PasswordHash, @PhoneNumber, @DateOfBirth, @Address);"; // Assuming Username is FirstName for Customer
                            parameters.Add("DateOfBirth", user.DateOfBirth, DbType.Date);
                            parameters.Add("Address", user.Address, DbType.String);
                            await connection.ExecuteAsync(customerQuery, parameters);
                            break;
                        case "InsuranceAgent":
                            var agentQuery = @"INSERT INTO InsuranceAgentRegistration (FirstName, Email, PasswordHash, PhoneNumber, CreatedDate) 
                                       VALUES (@Username, @Email, @PasswordHash, @PhoneNumber, @CreatedDate);"; // Assuming Username is FirstName for InsuranceAgent
                            await connection.ExecuteAsync(agentQuery, parameters);
                            break;
                        default:
                            throw new Exception("Invalid role specified");
                    }
                    return true;
                }
                return false;
            }
        }


        public async Task<string> UserLogin(string email, string password, IConfiguration configuration)
        {
            try
            {
                var query = "SELECT * FROM Users WHERE Email=@Email";
                using (var connection = _context.CreateConnection())
                {
                    var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });

                    // Check if user exists
                    if (user != null)
                    {
                        // Hash the provided password
                        string hashedPassword = HashPassword(password);

                        // Compare the hashed password with the stored hashed password
                        if (hashedPassword == user.PasswordHash)
                        {
                            // Generate JWT token
                            var token = GenerateJwtToken(user);
                            return token;
                        }
                    }

                    // Authentication failed
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while attempting login for user with email: {Email}", email);
                throw;
            }
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
    }
}
