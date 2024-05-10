using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.DTOs;

namespace Talabat.Helpers
{
    public class MappingProfiles :Profile
    {


        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.ProductType, O => O.MapFrom(P => P.ProductType.Name))
                .ForMember(d => d.ProductBrand, O => O.MapFrom(P => P.ProductBrand.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
                ;
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<Core.Entities.Order_Aggregate.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod , o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d=>d.DeliveryMethodCost , o=>o.MapFrom(s=>s.DeliveryMethod.Cost));
            CreateMap<Orderitem, OrderitemDto>()
                .ForMember(d=>d.ProductId , o=>o.MapFrom(s=>s.Product.ProductId))
                .ForMember(d=>d.ProductName , o=>o.MapFrom(s=>s.Product.ProductName))
                .ForMember(d=>d.PictureUrl , o=>o.MapFrom(s=>s.Product.PictureUrl))
                .ForMember(d=>d.PictureUrl , o=>o.MapFrom<OrderItemPictureUrlResolver>()); 

                

        }

    }    
}
