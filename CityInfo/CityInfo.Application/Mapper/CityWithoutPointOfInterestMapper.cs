using CityInfo.Application.Dto;
using CityInfo.Domain;

namespace CityInfo.Application.Mapper
{
    public class CityWithoutPointOfInterestMapper
    {
        public CityWithoutPointsOfInterestDto Map(City city)
        {
            CityWithoutPointsOfInterestDto cityWithoutPointOfInterestDto = new CityWithoutPointsOfInterestDto()
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };

            return cityWithoutPointOfInterestDto;
        }
    }
}

/* using AutoMapper
 * 
 * namsespace CityInfo.API.Profiles
 * {
 *      public class CityProfile:  Profile
 *      {
 *             public CityProfile()
 *             {
 *                  CreateMap<Entities.City, Models.CItyWithoutPointOfInterestDto>();
 *             }
 *      }
 *  }
 */
