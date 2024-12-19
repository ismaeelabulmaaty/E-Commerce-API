using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Core.Specifications.OrderSpecification
{
    public class OrderWithPaymentSpecifications:BaseSpecification<Order>
    {

        public OrderWithPaymentSpecifications(string paymentIntentId):base(O=>O.PaymentIntentId==paymentIntentId)
        {
            
        }

    }
}
