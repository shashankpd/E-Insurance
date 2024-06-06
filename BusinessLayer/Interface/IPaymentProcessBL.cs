using ModelLayer.Entity;
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

    }
}
