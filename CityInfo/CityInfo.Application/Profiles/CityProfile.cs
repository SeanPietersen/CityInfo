using AutoMapper;

namespace CityInfo.Application.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Domain.City, Application.Dto.CityDto>();
            CreateMap<Domain.City, Application.Dto.CityWithPointOfInterestDto>();
            CreateMap<Application.Dto.PointOfInterestForCreationDto, Domain.PointOfInterest>();
        }
    }
}
