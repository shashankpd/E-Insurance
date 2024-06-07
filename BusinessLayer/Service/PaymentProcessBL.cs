using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class PaymentProcessBL : IPaymentProcessBL
    {
        private readonly IPaymentProcessService PaymentProcess;

        public PaymentProcessBL(IPaymentProcessService PaymentProcess)
        {
            this.PaymentProcess = PaymentProcess;
        }

        //start

        public Task<bool> AddPayment(Payment Payment)
        {
            return PaymentProcess.AddPayment(Payment);
        }
        public Task<IEnumerable<PaymentModel>> GetAllPayments()
        {
            return PaymentProcess.GetAllPayments();
        }
        
        public Task<IEnumerable<PaymentModel>> GetPaymentById(int CustomerId)
        {
            return PaymentProcess.GetPaymentById(CustomerId);
        }

        public Task<IEnumerable<ReceiptDetails>> GetRecieptByPaymementId(int PaymentId)
        {
            return PaymentProcess.GetRecieptByPaymementId(PaymentId);
        }
        public Task<decimal> CalculatePremium(int policyId, int customerAge, decimal coverageAmount, int termLength, string policyType)
        {
            return PaymentProcess.CalculatePremium(policyId, customerAge, coverageAmount, termLength, policyType);
        }
    }
}
