using CityInfo.Application.Contract;
using CityInfo.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public ActionResult<IEnumerable<CityDto>> GetAllCities()
        {

            var cities = _cityContract.GetAllCities().Result;
            return Ok(cities);
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
