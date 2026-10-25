using AutoMapper;
using World.Api.DTO.Country;
using World.Api.DTO.States;
using World.Api.Models;

namespace World.Api.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Country,CreateCountryDto>().ReverseMap();
            CreateMap<Country,CountryDto>().ReverseMap();
            CreateMap<Country,UpdateCountryDto>().ReverseMap();

            CreateMap<States, StatesDto>().ReverseMap();
            CreateMap<States, CreateStatesDto>().ReverseMap();
            CreateMap<States, UpdateStatesDto>().ReverseMap();
        }
    }
}
