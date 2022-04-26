using CityInfo.Application;
using CityInfo.Application.Contract;
using CityInfo.Application.Dto;
using CityInfo.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize(Policy = "MustBeFromCapeTown")]
    [ApiVersion("2.0")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PointsOfInterestController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IPointOfInterestContract _pointOfInterestContract;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                          IPointOfInterestContract pointOfInterestContract, 
                                          IMailService mailService,
                                          ICityInfoRepository cityInfoRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pointOfInterestContract = pointOfInterestContract ?? throw new ArgumentNullException(nameof(pointOfInterestContract));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository;
        }


        /// <summary>
        /// Get all the points of interest
        /// </summary>
        /// <param name="cityId">City id of the points of interest to get from</param>
        /// <returns>IAction Result</returns>
        /// <response code="200">Returns the requested points of interest</response>
        /// <response code="404">Points of interest were not found</response>
        /// <response code="403">City Name does not match city Id</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetAllPointsOfInterest(int cityId)
        {
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
            if (!_cityInfoRepository.CityNameMatchesCityId(cityName, cityId).Result)
            {
                return Forbid();
            }

            var pointOfInterests = _pointOfInterestContract.GetAllPointsOfInterestByCityId(cityId);

            if (pointOfInterests == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterests);
        }


        /// <summary>
        /// Get one point of interest for a city
        /// </summary>
        /// <param name="cityId">City id for point of interest to return</param>
        /// <param name="pointOfInterestId">Point of interest id for point of interest to return</param>
        /// <returns>IAction Result</returns>
        /// <response code="200">Returns the requested point of interest</response>
        /// <response code="404">Point of interest were not found</response>
        
        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var pointOfInterest = _pointOfInterestContract.GetPointOfInterestById(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }


        /// <summary>
        /// Create point of interest
        /// </summary>
        /// <param name="cityId">City id for point of interest to create</param>
        /// <param name="pointOfInterest">Point of interest id for point of interest to create</param>
        /// <returns>IAction Result</returns>
        /// <response code="200">Created point of interest successful</response>
        /// <response code="404">Point of interest to create was not found</response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// Update point of interest
        /// </summary>
        /// <param name="cityId">City id for point of interest to update</param>
        /// <param name="pointOfInterestId">Point of interest id for the point of interest to update</param>
        /// <param name="pointOfInterest">Point of interest to be updated</param>
        /// <returns>IAction Result</returns>
        /// <response code="204">Content was updated</response>
        /// <response code="404">Point of interest to update was not found</response>

        [HttpPut("{pointofinterestid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// Partially Update point of interest
        /// </summary>
        /// <param name="cityId">City id for point of interest to partially update</param>
        /// <param name="pointOfInterestId">Point of interest id for point of interest to partially update</param>
        /// <param name="patchDocumentDto">Partially updated point of interest</param>
        /// <returns>IAction Result</returns>
        /// <response code="204">Content was partially updated</response>
        /// <response code="404">Point of interest to partially update was not found</response>
        
        [HttpPatch("{pointofinterestid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// Delete point of interest
        /// </summary>
        /// <param name="cityId">City id for point of interest to delete</param>
        /// <param name="pointOfInterestId">Point of interest id for point of interest to delete</param>
        /// <returns>IAction Result</returns>
        /// <response code="204">Point of interest was deleted</response>
        /// <response code="404">Point of interest to be deleted was not found</response>

        [HttpDelete("{pointOfInterestId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
