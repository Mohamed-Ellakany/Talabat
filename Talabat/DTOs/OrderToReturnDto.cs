using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; } 

        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }//  name 

        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderitemDto> Items { get; set; } = new HashSet<OrderitemDto>();

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntentId { get; set; } 




    }
}
