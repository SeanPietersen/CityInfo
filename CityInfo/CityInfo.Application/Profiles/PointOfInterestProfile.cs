using AutoMapper;

namespace CityInfo.Application.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Domain.PointOfInterest, Application.Dto.PointOfInterestDto>();
            CreateMap<Application.Dto.PointOfInterestForCreationDto, Domain.PointOfInterest>();
            CreateMap<Application.Dto.PointOfInterestForUpdateDto, Domain.PointOfInterest>();
            CreateMap<Domain.PointOfInterest, Application.Dto.PointOfInterestForUpdateDto>();
        }
    }
}
