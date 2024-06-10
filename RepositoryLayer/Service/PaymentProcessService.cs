using Dapper;
using ModelLayer.Entity;
<<<<<<< HEAD
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
=======
using ModelLayer.RequestDTO;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using StackExchange.Redis;
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class PaymentProcessService : IPaymentProcessService
<<<<<<< HEAD

    {
        private readonly DapperContext _context;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public PaymentProcessService(DapperContext context)
        {
            _context = context;
        }
        //start
=======
    {
        private readonly DapperContext _context;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabase _cache;


        public PaymentProcessService(DapperContext context, ConnectionMultiplexer redisConnection)
        {
            _context = context;
            _cache = redisConnection.GetDatabase();
        }
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f

        public async Task<bool> AddPayment(Payment payment)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("CustomerPolicyId", payment.CustomerPolicyId);
                parameters.Add("PaymentDate", payment.PaymentDate);
                parameters.Add("Amount", payment.Amount);

                _logger.Info($"Attempting to validate payment: CustomerPolicyId={payment.CustomerPolicyId}, PaymentDate={payment.PaymentDate}, Amount={payment.Amount}");

                var rowsAffected = await _context.CreateConnection().ExecuteAsync("sp_ValidatePayment", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    _logger.Info($"Payment processed successfully");
                    return true;
                }
                else
                {
                    _logger.Warn($"Payment validation failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while processing payment");
                throw;
            }
        }

<<<<<<< HEAD
=======
        public async Task<IEnumerable<PaymentModel>> GetAllPayments()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var payments = await connection.QueryAsync<PaymentModel>("sp_GetAllPayments", commandType: CommandType.StoredProcedure);
                    return payments;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving payments");
                throw;
            }
        }

        public async Task<IEnumerable<PaymentModel>> GetPaymentById(int CustomerId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CustomerId", CustomerId); // Add CustomerId parameter

                using (var connection = _context.CreateConnection())
                {
                    var payments = await connection.QueryAsync<PaymentModel>("sp_GetPaymentsByCustomerId", parameters, commandType: CommandType.StoredProcedure);
                    return payments;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving payments");
                throw;
            }
        }

        public async Task<IEnumerable<ReceiptDetails>> GetRecieptByPaymementId(int paymentId)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("PaymentId", paymentId, DbType.Int32);

                    var receipts = await connection.QueryAsync<ReceiptDetails>(
                        "sp_GenerateReceipt",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return receipts;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while Generating Reciept");
                throw;
            }
        }

        public async Task<decimal> CalculatePremium(int policyId, int customerAge, decimal coverageAmount, string policyType, string paymentFrequency,int TermYears)
        {
            try
            {
                string cacheKey = $"premium:{policyId}:{customerAge}:{coverageAmount}:{policyType}:{paymentFrequency}:{TermYears}";

                var cachedPremium = await _cache.StringGetAsync(cacheKey);

                if (cachedPremium.HasValue)
                {
                    _logger.Info("Premium retrieved from cache");
                    return (decimal)cachedPremium;
                }

                var parameters = new DynamicParameters();
                parameters.Add("PolicyId", policyId);
                parameters.Add("CustomerAge", customerAge);
                parameters.Add("CoverageAmount", coverageAmount);
                parameters.Add("PolicyType", policyType);
                parameters.Add("paymentFrequency",paymentFrequency);
                parameters.Add("TermYears", TermYears);

                using (var connection = _context.CreateConnection())
                {
                    // Execute the stored procedure to get the premium value
                    var premium = await connection.QueryFirstOrDefaultAsync<decimal>("CalculatePremium", parameters, commandType: CommandType.StoredProcedure);

                    // Cache the premium for future requests
                    await _cache.StringSetAsync(cacheKey, premium.ToString(), TimeSpan.FromHours(1));

                    _logger.Info("Premium calculated and cached");
                    return premium;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while calculating premium");
                throw;
            }
        }
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f

    }
}
