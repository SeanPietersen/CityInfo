using CityInfo.Application.Contract;
using CityInfo.Application.Dto;
using CityInfo.Domain;
using CityInfo.Infrastructure;
using CityInfo.Infrastructure.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CityInfo.Test
{
    public class PointOfInterestContractTest
    {
        private readonly IPointOfInterestContract _pointOfInterest;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointOfInterestContractTest()
        {
            _mailService = Substitute.For<IMailService>();
            _cityInfoRepository = Substitute.For<ICityInfoRepository>();
            _pointOfInterest = new PointOfInterestContract(null, _mailService, _cityInfoRepository);
        }

        [Fact]
        public void GetAllPointsOfInterestByCityId_ShouldReturnNull_WhenCityIdIsInvalid()
        {
            //Arrange         
            int cityId = 0;

            _cityInfoRepository.GetAllPointsOfInterestForCityAsync(cityId).ReturnsNull();

            //Act
            IEnumerable<PointOfInterestDto> actual = _pointOfInterest.GetPointsOfInterestByCityId(cityId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetPointsOfInterestByCityId_IsSuccessful()
        {
            //Arrange
            int cityId = 1;

            var expected = new List<PointOfInterest>()
            {
                new PointOfInterest()
                {
                    Id = 1,
                    Name = "Central Park",
                    Description = "The most visited urban park in the United States"
                },
                new PointOfInterest()
                {
                    Id = 2,
                    Name = "Empire State Building",
                    Description = "A 102-story skyscraper located in Midtown Manhattan"
                }
            };

            _cityInfoRepository.GetAllPointsOfInterestForCityAsync(cityId).Returns(expected);

            //Act
            IEnumerable<PointOfInterestDto> actual = _pointOfInterest.GetPointsOfInterestByCityId(cityId);

            //Assert
            Assert.Equal(expected.Count, actual.ToList().Count);
            Assert.Equal(expected[0].Name, actual.ToList()[0].Name);
        }

        [Fact]
        public void GetPointOfInterestById_ShouldReturnNull_WhenCityIdIsInvalid()
        {
            //Arrange         
            int cityId = 0;

            int pointOfInterestId = 1;

            _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).ReturnsNull();

            //Act
            PointOfInterestDto actual = _pointOfInterest.GetPointOfInterestById(cityId, pointOfInterestId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetPointOfInterestById_ShouldReturnNull_WhenPointOfInterestIdIsInvalid()
        {
            //Arrange

            int cityId = 1;

            int pointOfInterestId = 0;

            _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).ReturnsNull();

            //Act
            PointOfInterestDto actual = _pointOfInterest.GetPointOfInterestById(cityId, pointOfInterestId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetPointOfInterestById_IsSuccessful()
        {
            //Arrange
            var pointOfInterestInDb = new PointOfInterest()
            {
                Id = 1,
                Name = "Central Park",
                Description = "The most visited urban park in the United States"
            };

            int cityId = 1;

            int pointOfInterestId = 1;

            _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Returns(pointOfInterestInDb);

            //Act
            PointOfInterestDto actual = _pointOfInterest.GetPointOfInterestById(cityId, pointOfInterestId);

            //Assert
            Assert.Equal(pointOfInterestInDb.Name, actual.Name);
            Assert.Equal(pointOfInterestInDb.Description, actual.Description);
        }

        [Fact]
        public void CreatePointOfInterestById_ShouldReturnNull_WhenCityIdIsInvalid()
        {
            //Arrange         
            int cityId = 0;

            PointOfInterestForCreationDto pointOfInterest = new PointOfInterestForCreationDto()
            {
                Name = "Central Park",
                Description = "The most visited urban park in the United States"
            };

            _cityInfoRepository.GetCityByCityIdAsync(cityId, true).ReturnsNull();

            //Act
            PointOfInterestDto actual = _pointOfInterest.CreatePointOfInterestById(cityId, pointOfInterest);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void CreatePointOfInterestById_IsSuccessful()
        {
            //Arrange
            int cityId = 1;

            var cityInDb = new City()
            {
                Id = 1,
                Name = "New York City",
                Description = "The one with that big park",
            };

            var pointOfInterestInDb = new List<PointOfInterest>()
            {
                new PointOfInterest()
                {
                    Id = 1,
                    Name = "Central Park",
                    Description = "The most visited urban park in the United States"
                },
                new PointOfInterest()
                {
                    Id = 2,
                    Name = "Empire State Building",
                    Description = "A 102-story skyscraper located in Midtown Manhattan",
                    City = cityInDb,
                    CityId = cityInDb.Id
                }
            };

            PointOfInterestForCreationDto pointOfInterest = new PointOfInterestForCreationDto()
            {
                Name = "Big Apple",
                Description = "The most visited attraction site in the New York"
            };

            _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Returns(cityInDb);
            _cityInfoRepository.GetAllPointsOfInterestForCityAsync(cityId).Returns(pointOfInterestInDb);

            //Act
            PointOfInterestDto actual = _pointOfInterest.CreatePointOfInterestById(cityId, pointOfInterest);

            //Assert
            Assert.Equal(pointOfInterest.Name, actual.Name);
            Assert.Equal(pointOfInterest.Description, actual.Description);
        }

        [Fact]
        public void UpdatePointOfInterestById_ShouldReturnNull_WhenCityIdIsInvalid()
        {
            //Arrange
            int cityId = 0;

            int pointOfInterestId = 2;

            _cityInfoRepository.GetCityByCityIdAsync(cityId,true).ReturnsNull();

            //Act
            PointOfInterestDto actual = _pointOfInterest.UpdatePointOfInterestById(cityId, pointOfInterestId, null);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void UpdatePointOfInterestById_ShouldReturnNull_WhenPointOfInterestIdIsInvalid()
        {
            //Arrange

            int cityId = 1;

            int pointOfInterestId = 0;

            var cityInDb = new City()
            {
                Id = 1,
                Name = "New York City",
                Description = "The one with that big park",
                PointOfInterest = new List<PointOfInterest>()
                {
                    new PointOfInterest()
                    {
                        Id = 1,
                        Name = "Central Park",
                        Description = "The most visited urban park in the United States"
                    },
                    new PointOfInterest()
                    {
                        Id = 2,
                        Name = "Empire State Building",
                        Description = "A 102-story skyscraper located in Midtown Manhattan"
                    }
                }
            };

            _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Returns(cityInDb);
            _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).ReturnsNull();

            //Act
            PointOfInterestDto actual =  _pointOfInterest.UpdatePointOfInterestById(cityId, pointOfInterestId, null);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void UpdatePointOfInterestById_IsSuccessful()
        {
            //Arrange
            var cityInDb = new City()
            {
                Id = 1,
                Name = "New York City",
                Description = "The one with that big park",
                PointOfInterest = new List<PointOfInterest>()
                {
                    new PointOfInterest()
                    {
                        Id = 1,
                        Name = "Central Park",
                        Description = "The most visited urban park in the United States"
                    },
                    new PointOfInterest()
                    {
                        Id = 2,
                        Name = "Empire State Building",
                        Description = "A 102-story skyscraper located in Midtown Manhattan"
                    }
                }
            };

            var pointOfInterestInDB = new PointOfInterest()
            {
                Id = 1,
                Name = "Central Park",
                Description = "The most visited urban park in the United States"
            };

            var pointOfInterestForUpdateDto = new PointOfInterestForUpdateDto()
            {
                Name = "Updated - Central Park",
                Description = "Updated - The most visited urban park in the United States."
            };

            int cityId = 1;

            int pointOfInterestId = 1;

            _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Returns(cityInDb);
            _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Returns(pointOfInterestInDB);

            //Act
            PointOfInterestDto actual = _pointOfInterest.UpdatePointOfInterestById(cityId, pointOfInterestId, pointOfInterestForUpdateDto);

            //Assert
            Assert.Equal(pointOfInterestForUpdateDto.Name, actual.Name);
            Assert.Equal(pointOfInterestForUpdateDto.Description, actual.Description);
        }

        [Fact]
        public void DeletePointOfInterestById_ShouldReturnNull_WhenCityIdIsInvalid()
        {
            //Arrange
            var cityId = 0;

            bool mailSent = false;

            int pointOfInterestId = 1;

            _cityInfoRepository.GetCityByCityIdAsync(cityId, true).ReturnsNull();

            _mailService.When(x => x.Send(Arg.Any<string>(), Arg.Any<string>())).Do(x => mailSent = true);

            //Act
            PointOfInterestDto actual = _pointOfInterest.DeletePointOfInterestById(cityId, pointOfInterestId);

            //Assert
            Assert.Null(actual);
            Assert.False(mailSent);
        }

        [Fact]
        public void DeletePointOfInterestById_ShouldReturnNull_WhenPointOfInterestIdIsInvalid()
        {
            //Arrange

            int cityId = 1;

            int pointOfInterestId = 0;

            bool mailSent = false;

            var cityInDb = new City()
            {
                Id = 1,
                Name = "MOCK",
                Description = "TEST",
                PointOfInterest = new List<PointOfInterest>()
                {
                    new PointOfInterest()
                    {
                        Id = 1,
                        Name = "Central Park",
                        Description = "The most visited urban park in the United States"
                    },
                    new PointOfInterest()
                    {
                        Id = 2,
                        Name = "Empire State Building",
                        Description = "A 102-story skyscraper located in Midtown Manhattan"
                    }
                }
            };

            _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Returns(cityInDb);
            _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).ReturnsNull();

            _mailService.When(x => x.Send(Arg.Any<string>(), Arg.Any<string>())).Do(x => mailSent = true);

            //Act
            PointOfInterestDto actual = _pointOfInterest.DeletePointOfInterestById(cityId, pointOfInterestId);

            //Assert
            Assert.Null(actual);
            Assert.False(mailSent);
        }

        [Fact]
        public void DeletePointOfInterestById_IsSuccessful()
        {
            //Arrange
            int cityId = 1;

            bool mailSent = false;

            int pointOfInterestId = 1;

            var expected = new PointOfInterest()
            {
                Id = 1,
                Name = "Central Park",
                Description = "The most visited urban park in the United States"
            };

            var cityInDb = new City()
            {
                Id = 1,
                Name = "MOCK",
                Description = "TEST",
                PointOfInterest = new List<PointOfInterest>()
                {
                    new PointOfInterest()
                    {
                        Id = 1,
                        Name = "Central Park",
                        Description = "The most visited urban park in the United States"
                    },
                    new PointOfInterest()
                    {
                        Id = 2,
                        Name = "Empire State Building",
                        Description = "A 102-story skyscraper located in Midtown Manhattan"
                    }
                }
            };

            _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Returns(cityInDb);

            _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Returns(expected);

            _mailService.When(x => x.Send(Arg.Any<string>(), Arg.Any<string>())).Do(x => mailSent = true);

            //Act
            PointOfInterestDto actual = _pointOfInterest.DeletePointOfInterestById(cityId, pointOfInterestId);

            //Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.True(mailSent);
        }


    }
}
