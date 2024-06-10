using ModelLayer.Entity;
<<<<<<< HEAD
=======
using ModelLayer.RequestDTO;
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IPaymentProcessService
    {
        public Task<bool> AddPayment(Payment Payment);

<<<<<<< HEAD
=======
        public Task<IEnumerable<PaymentModel>> GetAllPayments();

        public Task<IEnumerable<PaymentModel>> GetPaymentById(int CustomerId);

        public Task<IEnumerable<ReceiptDetails>> GetRecieptByPaymementId(int PaymentId);
        public Task<decimal> CalculatePremium(int policyId, int customerAge, decimal coverageAmount, string policyType, string paymentFrequency,int TermYears);
        public Task FinalizePurchase();






>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
    }
}
