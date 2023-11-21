using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.APIS.DTOS
{
    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}