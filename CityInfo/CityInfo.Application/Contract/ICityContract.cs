using CityInfo.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public interface ICityContract
    {
        Task<IEnumerable<CityDto>> GetAllCities();
        Task<CityDto> GetCityById(int id, bool includePointsOfInterest = false);
    }
}
