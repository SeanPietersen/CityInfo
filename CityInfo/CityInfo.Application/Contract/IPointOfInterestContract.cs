using CityInfo.Application.Dto;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public interface IPointOfInterestContract
    {
        IEnumerable<PointOfInterestDto> GetPointsOfInterestByCityId(int cityId);
        PointOfInterestDto GetPointOfInterestById(int cityId, int pointOfInterestId);

        PointOfInterestDto CreatePointOfInterestById(int cityId, PointOfInterestForCreationDto pointOfInterest);

        PointOfInterestDto UpdatePointOfInterestById(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest);

        PointOfInterestDto PartiallUpdatePointOfInterestById(int cityId,
                                                             int pointOfInterestId,
                                                             JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument);

        PointOfInterestDto DeletePointOfInterestById(int cityId, int pointOfInterestId);

    }
}
