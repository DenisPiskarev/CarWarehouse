using AutoMapper;
using CarWarehouse.BLL.DTO;
using CarWarehouse.DAL.Models;
using CarWarehouse.Web.ViewModels;

namespace CarWarehouse.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarViewModel>().ReverseMap();

            CreateMap<AuthenticateRequest, AuthenticateViewModel>().ReverseMap();

            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<AuthenticateResponse, AuthenticateViewModel>();
        }
    }
}
