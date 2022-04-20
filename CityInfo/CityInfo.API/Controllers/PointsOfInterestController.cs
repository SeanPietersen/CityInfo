using CityInfo.Application;
using CityInfo.Application.Contract;
using CityInfo.Application.Dto;
using CityInfo.Infrastructure.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IPointOfInterestContract _pointOfInterestContract;
        private readonly IMailService _mailService;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
                                          IPointOfInterestContract pointOfInterestContract, IMailService mailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pointOfInterestContract = pointOfInterestContract ?? throw new ArgumentNullException(nameof(pointOfInterestContract));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetAllPointsOfInterest(int cityId)
        {
            var pointOfInterests = _pointOfInterestContract.GetAllPointsOfInterestByCityId(cityId);

            if (pointOfInterests == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterests);
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var pointOfInterest = _pointOfInterestContract.GetPointOfInterestById(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            var createdPointOfInterest = _pointOfInterestContract.CreatePointOfInterestById(cityId, pointOfInterest).Result;

            if (createdPointOfInterest == null)
            {
                return NotFound();
            }

            //return CreatedAtRoute("GetPointOfInterest", createdPointOfInterest);
            return Ok(createdPointOfInterest);
        }

        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {

            var pointOfInterestFromStore = _pointOfInterestContract.UpdatePointOfInterestById(cityId,
                                                                                            pointOfInterestId,
                                                                                            pointOfInterest);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocumentDto)
        {

            var pointOfInterestFromStoreDto = _pointOfInterestContract.PartiallUpdatePointOfInterestById(cityId,
                                                                                                      pointOfInterestId,
                                                                                                      patchDocumentDto);
            if (pointOfInterestFromStoreDto == null)
            {
                return NotFound();
            }

            //patchDocumentDto.ApplyTo(pointOfInterestToPatch, ModelState);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (!TryValidateModel(pointOfInterestToPatch))
            //{
            //    return BadRequest(ModelState);
            //}

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var pointOfInterestFromStore = _pointOfInterestContract.DeletePointOfInterestById(cityId, pointOfInterestId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
