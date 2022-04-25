using CityInfo.Application.Dto;
using CityInfo.Domain;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public interface IPointOfInterestContract
    {
        IEnumerable<PointOfInterestDto> GetAllPointsOfInterestByCityId(int cityId);
        PointOfInterestDto GetPointOfInterestById(int cityId, int pointOfInterestId);

        Task<PointOfInterestDto> CreatePointOfInterestById(int cityId, PointOfInterestForCreationDto pointOfInterest);

        Task<PointOfInterestDto> UpdatePointOfInterestById(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest);

        Task<PointOfInterestDto> PartiallUpdatePointOfInterestById(int cityId,
                                                             int pointOfInterestId,
                                                             JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument);

        Task<PointOfInterestDto> DeletePointOfInterestById(int cityId, int pointOfInterestId);

    }
}
