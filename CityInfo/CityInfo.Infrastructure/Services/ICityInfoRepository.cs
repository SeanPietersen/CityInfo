using CityInfo.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Infrastructure.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityByCityIdAsync(int cityId, bool includePointsOfInterest);
        Task<bool> CityForCityIdExists(int cityId);
        Task<IEnumerable<PointOfInterest>> GetAllPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityByPointOfInterestIdAsync(int cityId, int pointOfInterestId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        bool UpdatePointOfInterestForCity(PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
    }
}
