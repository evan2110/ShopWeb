using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;

namespace ShoppingWebAPI.Config;

public class MapperConfigs : Profile
{
    public MapperConfigs()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
            .ReverseMap();

    }
}