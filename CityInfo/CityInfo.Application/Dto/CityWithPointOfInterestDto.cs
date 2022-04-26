using System.Collections.Generic;

namespace CityInfo.Application.Dto
{
    /// <summary>
    /// A DTO for a city with points of interest
    /// </summary>
    public class CityWithPointOfInterestDto: CityDto
    {
        /// <summary>
        /// Points of interest for a city
        /// </summary>
        public int NumberOfPointsOfInterest
        {
            get
            {
                return PointOfInterest.Count;
            }
        }
        public ICollection<PointOfInterestDto> PointOfInterest { get; set; } = new List<PointOfInterestDto>();
    }
}
