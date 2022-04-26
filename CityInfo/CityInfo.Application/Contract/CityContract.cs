using AutoMapper;
using CityInfo.Application.Dto;
using CityInfo.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public class CityContract : ICityContract
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CityContract(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        public async Task<(IEnumerable<CityDto>, PaginationMetadata)> GetAllCities(
            string name, string searchQuery, int pageNumber, int pageSize)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            return (_mapper.Map<IEnumerable<CityDto>>(cityEntities), paginationMetadata);
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
