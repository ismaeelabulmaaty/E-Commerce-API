using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;

namespace Talabat.Core.Services.Contract
{
    public interface IpymentService
    {

        // functiion to create or updtate pyment intent
        Task<CustomerBasket?> CreateOrUpdatePymentIntent(string BasketId);

        Task<Order> UpdatePaymentIntentToSucceedOrfailed(string PaymentIntent, bool flag);

    }
}
