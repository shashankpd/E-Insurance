using BusinessLayer.Interface;
using ModelLayer.Entity;
<<<<<<< HEAD
=======
using ModelLayer.RequestDTO;
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
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
<<<<<<< HEAD
=======
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
        public Task<decimal> CalculatePremium(int policyId, int customerAge, decimal coverageAmount, string policyType, string paymentFrequency, int TermYears)
        {
            return PaymentProcess.CalculatePremium(policyId, customerAge, coverageAmount, policyType,paymentFrequency, TermYears);
        }
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
    }
}
