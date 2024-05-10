using AutoMapper;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.DTOs;

namespace Talabat.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<Orderitem, OrderitemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Orderitem source, OrderitemDto destination, string destMember, ResolutionContext context)
        {

            

            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
