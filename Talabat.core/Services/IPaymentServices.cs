using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;
using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.core.Services
{
    public interface IPaymentServices
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
        Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag);
    }
}
