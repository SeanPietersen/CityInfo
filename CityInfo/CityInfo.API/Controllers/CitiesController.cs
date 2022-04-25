using CityInfo.Application.Contract;
using CityInfo.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityContract _cityContract;

        public CitiesController(ICityContract cityContract)
        {
            _cityContract = cityContract;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetAllCities([FromQuery] string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {

            var cities = _cityContract.GetAllCities(name, searchQuery, pageNumber, pageSize).Result;

            //Is there a better way in using the "Item1 & Item2"
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(cities.Item2));

            return Ok(cities.Item1);
        }

        [HttpGet("{id}")]
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
