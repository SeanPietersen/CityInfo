using CityInfo.Application.Contract;
using CityInfo.Domain;
using CityInfo.Infrastructure.Services;
using NSubstitute;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute.ReturnsExtensions;
using AutoMapper;
using CityInfo.Application.Profiles;

namespace CityInfo.Test
{
    public class CityContractTest: ContextTest
    {
        private readonly ICityContract _sut;
        private readonly ICityInfoRepository _cityInfoRepository;
        

        public CityContractTest()
        {
            _cityInfoRepository = Substitute.For<ICityInfoRepository>();
            
            //sut = system under test
            _sut = new CityContract(_cityInfoRepository, _mapper);
        }

        //[Fact]
        //public async Task GetAllCities_IsSuccessfulAsync()
        //{
        //    //Arrange
        //    var cityInDb = new List<City>()
        //    {
        //        new City()
        //        {
        //            Id = 1,
        //            Name = "MOCK",
        //            Description = "TEST",
        //            PointOfInterest = new List<PointOfInterest>()
        //                {
        //                    new PointOfInterest()
        //                    {
        //                        Id = 1,
        //                        Name = "Central Park",
        //                        Description = "The most visited urban park in the United States"
        //                    },
        //                    new PointOfInterest()
        //                    {
        //                        Id = 2,
        //                        Name = "Empire State Building",
        //                        Description = "A 102-story skyscraper located in Midtown Manhattan"
        //                    }
        //                }
        //        },
        //        new City()
        //        {
        //            Id = 2,
        //            Name = "MOCK 2",
        //            Description = "TEST",
        //            PointOfInterest = new List<PointOfInterest>()
        //                {
        //                    new PointOfInterest()
        //                    {
        //                        Id = 1,
        //                        Name = "Central Park",
        //                        Description = "The most visited urban park in the United States"
        //                    },
        //                    new PointOfInterest()
        //                    {
        //                        Id = 2,
        //                        Name = "Empire State Building",
        //                        Description = "A 102-story skyscraper located in Midtown Manhattan"
        //                    }
        //                }
        //        }
        //    };

        //    _cityInfoRepository.GetCitiesAsync().Returns(cityInDb);

        //    //Act

        //    var actual = await _sut.GetAllCities();

        //    //Assert
        //    Assert.Equal(cityInDb.Count, actual.ToList().Count);

        //}

        [Fact]
        public async Task GetCityById_ShouldReturnNull_WhenCityIdIsInvalid()
        {
            //Arrange
            int cityId = 0;

            _cityInfoRepository.GetCityByCityIdAsync(cityId,false).ReturnsNull();

            //Act

            var actual = await _sut.GetCityById(cityId);

            //Assert
            Assert.Null(actual);

        }

        [Fact]
        public async Task GetCityById_IsSuccessfulAsync()
        {
            //Arrange
            var cityInDb = new City()
            {
                Id = 1,
                Name = "MOCK",
                Description = "Test"
            };

            int cityId = 1;

            _cityInfoRepository.GetCityByCityIdAsync(cityId, false).Returns(cityInDb);

            //Act

            var actual = await _sut.GetCityById(cityId);

            //Assert
            Assert.Equal(cityInDb.Name, actual.Name);
            Assert.Equal(cityInDb.Description, actual.Description);

        }

    }
}
