using BusinessLayer.Interface;
using ModelLayer.Entity;
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
    }
}
