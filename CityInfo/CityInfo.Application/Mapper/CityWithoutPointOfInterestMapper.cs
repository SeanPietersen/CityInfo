using CityInfo.Application.Dto;
using CityInfo.Domain;

namespace CityInfo.Application.Mapper
{
    public class CityWithoutPointOfInterestMapper
    {
        public CityDto Map(City city)
        {
            CityDto cityWithoutPointOfInterestDto = new CityDto()
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
 *                  CreateMap<Entities.City, Models.CityWithoutPointOfInterestDto>();
 *             }
 *      }
 *  }
 */
