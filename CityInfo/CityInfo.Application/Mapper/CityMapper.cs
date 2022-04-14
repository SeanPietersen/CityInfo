using CityInfo.Application.Dto;
using CityInfo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.Application.Mapper
{
    public class CityMapper
    {
        public CityWithPointOfIntrestDto Map(City city)
        {
            CityWithPointOfIntrestDto cityDto = new CityWithPointOfIntrestDto()
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };

            foreach (var pointOfInterest in city.PointOfInterest)
            {
                PointOfInterestDto dto = new PointOfInterestDto()
                {
                    Id = pointOfInterest.Id,
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description,
                };

                cityDto.PointOfInterest.Add(dto);
            }

            return cityDto;
        }
    }
}
