using System.Collections.Generic;

namespace CityInfo.Application.Dto
{
    public class CityWithPointOfIntrestDto: CityDto
    {
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
