using CityInfo.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public interface ICityContract
    {
        Task<IEnumerable<CityWithoutPointsOfInterestDto>> GetAllCities();
        Task<CityWithoutPointsOfInterestDto> GetCityById(int id);
    }
}
