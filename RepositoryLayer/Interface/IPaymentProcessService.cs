using ModelLayer.Entity;
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

    }
}
