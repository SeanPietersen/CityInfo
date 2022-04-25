using CityInfo.Application.Dto;
using CityInfo.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public interface ICityContract
    {
        //Task<IEnumerable<CityDto>> GetAllCities();
        Task<(IEnumerable<CityDto>, PaginationMetadata)> GetAllCities(string name, string searchQuery, int pageNumber = 1, int pageSize = 10);
        Task<CityDto> GetCityById(int id, bool includePointsOfInterest = false);
    }
}
