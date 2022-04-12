using CityInfo.Domain;
using System.Collections.Generic;

namespace CityInfo.Infrastructure
{
    public interface ICitiesDataStore
    {
        public List<City> Cities { get; set; }
    }
}
