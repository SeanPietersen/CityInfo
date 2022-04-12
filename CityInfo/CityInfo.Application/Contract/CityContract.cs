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
        private readonly CityWithoutPointOfInterestMapper _mapper = new CityWithoutPointOfInterestMapper();

        public CityContract(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        public async Task<IEnumerable<CityWithoutPointsOfInterestDto>> GetAllCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();

            var results = new List<CityWithoutPointsOfInterestDto>();

            foreach (var cityEntity in cityEntities)
            {
                var entity = _mapper.Map(cityEntity);

                results.Add(entity);
            }

            return results;
        }

        public async Task<CityWithoutPointsOfInterestDto> GetCityById(int id)
        {
            var city = await _cityInfoRepository.GetCityByCityIdAsync(id, false);

            if (city == null)
            {
                return null;
            }

            var result = _mapper.Map(city);

            return result;
        }
    }
}
