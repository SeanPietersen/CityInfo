using CityInfo.Domain;
using CityInfo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.Infrastructure.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            try
            {
                return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<City> GetCityByCityIdAsync(int cityId, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public  async Task<PointOfInterest> GetPointOfInterestForCityByPointOfInterestIdAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetAllPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(prop => prop.CityId == cityId).ToListAsync();
        }
    }
}
