using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string BasketId);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket Basket);
        Task<Boolean> DeleteBasketAsync(string BasketId);
    }
}
