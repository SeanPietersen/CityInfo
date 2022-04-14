using CityInfo.Application.Dto;
using CityInfo.Application.Mapper;
using CityInfo.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public class CityContract : ICityContract
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly CityWithoutPointOfInterestMapper _mapperWithoutPointOfInterest = new CityWithoutPointOfInterestMapper();
        private readonly CityMapper _mapperWithPointOfInterest = new CityMapper();

        public CityContract(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        public async Task<IEnumerable<CityDto>> GetAllCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();

            var results = new List<CityDto>();

            foreach (var cityEntity in cityEntities)
            {
                var entity = _mapperWithoutPointOfInterest.Map(cityEntity);

                results.Add(entity);
            }

            return results;
        }

        public async Task<CityDto> GetCityById(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityByCityIdAsync(id, includePointsOfInterest);

            if (city == null)
            {
                return null;
            }

            if (includePointsOfInterest)
            {
                return (_mapperWithPointOfInterest.Map(city));
            }

            return _mapperWithoutPointOfInterest.Map(city);
        }
    }
}
