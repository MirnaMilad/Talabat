using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.core.Specifications.OrderSpec
{
    public class OrderSpec:BaseSpecifications<Order>
    {
        public OrderSpec(string email) : base(O=>O.BuyerEmail ==email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        public OrderSpec(string email , int orderId) : base(O=>O.BuyerEmail ==email && O.Id == orderId) 
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
