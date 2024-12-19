using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Orders;
using Talabat.DTOs;

namespace Talabat.Helpar
{
    public class MappingProfil : Profile
    {
      

        public MappingProfil()
        {
            CreateMap<Product, ProductToReturnDto>()
                                .ForMember(D => D.Brand, O => O.MapFrom(S => S.Brand.Name))
                                .ForMember(D => D.Category, O => O.MapFrom(S => S.Category.Name))
                                .ForMember(p => p.PictureUrl, O => O.MapFrom<ProductPictureUrlResolve>());

            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressUserDto>().ReverseMap(); //Identity
            CreateMap<AddressUserDto , Talabat.Core.Entities.Orders.Address>().ReverseMap(); // order

            CreateMap<Order, OrderToReturnDto>()
                                .ForMember(D=>D.DeliveryMethod , O=>O.MapFrom(S=>S.DeliveryMethod.ShortName))
                                .ForMember(D => D.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                                .ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                                .ForMember(D=>D.PictureUrl , O=>O.MapFrom(S=>S.Product.PictureUrl))
                                .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderItemPictureResolver>());



        }

    }
}
