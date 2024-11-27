using AutoMapper;
using CarWarehouse.BLL.DTO;
using CarWarehouse.DAL.Models;

namespace CarWarehouse.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthenticateResponse>()
                .ForMember(dest => dest.JwtToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
        }
    }
}
