using CityInfo.Application.Contract;
using CityInfo.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityContract _cityContract;

        public CitiesController(ICityContract cityContract)
        {
            _cityContract = cityContract;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>> GetAllCities()
        {

            var cities = _cityContract.GetAllCities();
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCityById(int id)
        {
            //find city
            var city = _cityContract.GetCityById(id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }
}
