using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentIntentId):base(O=>O.PaymentIntentId==PaymentIntentId) { 
        
        }
    }
}
