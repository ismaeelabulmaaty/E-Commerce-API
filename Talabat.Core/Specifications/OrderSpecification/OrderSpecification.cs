using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Core.Specifications.OrderSpecification
{
    public class OrderSpecification : BaseSpecification<Order>
    {

        public OrderSpecification(string email) : base(O=>O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O => O.OrderDate);


        }

        public OrderSpecification( string email , int OrderId) : base(o=>o.BuyerEmail == email && o.Id == OrderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }

        

    }
}
