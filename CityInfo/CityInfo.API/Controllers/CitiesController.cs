using CityInfo.Application.Contract;
using CityInfo.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CitiesController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly ICityContract _cityContract;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CitiesController(ICityContract cityContract)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _cityContract = cityContract;
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        /// <param name="name">name to limit the collection of cities to return</param>
        /// <param name="searchQuery">adding matching items to the collection of cities to return</param>
        /// <param name="pageNumber">The number of cities to return</param>
        /// <param name="pageSize">The size of the cities to return</param>
        /// <returns>IAction Result</returns>
        /// <response code="200">Returns all the requested cities</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CityDto>> GetAllCities([FromQuery] string name, string searchQuery, int pageNumber = 1, int pageSize = 10)
        {

            var cities = _cityContract.GetAllCities(name, searchQuery, pageNumber, pageSize).Result;

            //Is there a better way in using the "Item1 & Item2"
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(cities.Item2));

            return Ok(cities.Item1);
        }


        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404">City was not found</response>
        /// <response code="400">A bad requested for a city</response>

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CityDto> GetCityById(int id, [BindRequired] bool includePointsOfInterest)
        {
            //find city
            var city = _cityContract.GetCityById(id, includePointsOfInterest).Result;

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }
}
