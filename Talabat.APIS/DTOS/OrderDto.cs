using System.ComponentModel.DataAnnotations;
using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.APIS.DTOS
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }

        public AddressDto shipToAddress { get; set; }
    }
}
