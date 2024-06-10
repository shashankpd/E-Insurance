using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IPaymentProcessBL
    {
        public Task<bool> AddPayment(Payment Payment);
        
        public Task<IEnumerable<PaymentModel>> GetAllPayments();

        public Task<IEnumerable<PaymentModel>> GetPaymentById(int CustomerId);

        public Task<IEnumerable<ReceiptDetails>> GetRecieptByPaymementId(int PaymentId);

        public Task<decimal> CalculatePremium(int policyId, int customerAge, decimal coverageAmount, string policyType, string paymentFrequency,int TermYears);
    }
}
