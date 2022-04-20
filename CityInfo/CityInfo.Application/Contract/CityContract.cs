using AutoMapper;
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
        private readonly IMapper _mapper;

        public CityContract(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CityDto>> GetAllCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();

            return _mapper.Map<IEnumerable<CityDto>>(cityEntities);
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
                return (_mapper.Map<CityWithPointOfInterestDto>(city));
            }

            return _mapper.Map<CityDto>(city);
        }
    }
}
