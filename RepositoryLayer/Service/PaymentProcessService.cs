using Dapper;
using ModelLayer.Entity;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class PaymentProcessService : IPaymentProcessService

    {
        private readonly DapperContext _context;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public PaymentProcessService(DapperContext context)
        {
            _context = context;
        }
        //start

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


    }
}
