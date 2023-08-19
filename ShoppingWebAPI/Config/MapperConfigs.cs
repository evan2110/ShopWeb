using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;

namespace ShoppingWebAPI.Config;

public class MapperConfigs: Profile
{
    public MapperConfigs()
    {
        CreateMap<Blog, BlogDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ReverseMap();

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ReverseMap();

        CreateMap<ProductColor, ProductColorDTO>()
            .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ColorName))
            .ReverseMap();

        CreateMap<ProductSize, ProductSizeDTO>()
            .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.SizeName))
            .ReverseMap();

        CreateMap<Cart, CartDTO>()
            .ReverseMap();

        CreateMap<CartItem, CartItemDTO>()
            .ForMember(dest => dest.ImageFront, opt => opt.MapFrom(src => src.Product.ImageFront))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
            .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ColorName))
            .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.SizeName))
            .ReverseMap();

        CreateMap<CartItemDTO, CartItem>();

        CreateMap<CartDTO, Cart>();

        CreateMap<Coupon, CouponDTO>()
            .ReverseMap();

        CreateMap<Order, OrderDTO>()
            .ReverseMap();

        CreateMap<OrderDetail, OrderDetailDTO>()
            .ReverseMap();

        CreateMap<Category, CategoryDTO>()
            .ReverseMap();

        CreateMap<Color, ColorDTO>()
            .ReverseMap();

        CreateMap<Rate, RateDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Email))
            .ReverseMap();

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
            .ReverseMap();

    }
}