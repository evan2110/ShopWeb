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
            .ReverseMap();

    }
}