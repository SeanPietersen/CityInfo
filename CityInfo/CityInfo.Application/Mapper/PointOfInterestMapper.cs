using CityInfo.Application.Dto;
using CityInfo.Domain;

namespace CityInfo.Application.Mapper
{
    public class PointOfInterestMapper
    {
        public PointOfInterestDto Map(PointOfInterest pointOfInterest)
        {
            PointOfInterestDto pointOfInterestDto = new PointOfInterestDto()
            {
                Id = pointOfInterest.Id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            return pointOfInterestDto;
        }
    }
}
