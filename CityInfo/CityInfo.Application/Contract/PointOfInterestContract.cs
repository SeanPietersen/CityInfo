using CityInfo.Application.Dto;
using CityInfo.Application.Mapper;
using CityInfo.Domain;
using CityInfo.Infrastructure.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.Application.Contract
{
    public class PointOfInterestContract : IPointOfInterestContract
    {
        private readonly ILogger<PointOfInterestContract> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly PointOfInterestMapper _mapper = new PointOfInterestMapper();

        public PointOfInterestContract(ILogger<PointOfInterestContract> logger,
                                       IMailService mailService,
                                       ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        public IEnumerable<PointOfInterestDto> GetAllPointsOfInterestByCityId(int cityId)
        {
            if(!_cityInfoRepository.CityForCityIdExists(cityId).Result)
            {
                _logger.LogCritical($"City with id {cityId} wasn't found when accessing points of interest.");
                return null;
            }

            var pointOfInterests = _cityInfoRepository.GetAllPointsOfInterestForCityAsync(cityId).Result;

            if (pointOfInterests == null)
            {
                return null;
            }

            PointOfInterestMapper pointOfInterestMapper = new PointOfInterestMapper();

            var pointOfInterestDtos = new List<PointOfInterestDto>();

            foreach (var pointOfInterest in pointOfInterests)
            {
                var pointOfInterestDto = pointOfInterestMapper.Map(pointOfInterest);
                pointOfInterestDtos.Add(pointOfInterestDto);
            }

            return pointOfInterestDtos;
        }

        public PointOfInterestDto GetPointOfInterestById(int cityId, int pointOfInterestId)
        {
            if (!_cityInfoRepository.CityForCityIdExists(cityId).Result)
            {
                _logger.LogCritical($"City with id {cityId} wasn't found when accessing points of interest.");
                return null;
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Result;

            if (pointOfInterest == null)
            {
                return null;
            }

            PointOfInterestMapper pointOfInterestMapper = new PointOfInterestMapper();

            var pointOfInterestDto = pointOfInterestMapper.Map(pointOfInterest);

            return pointOfInterestDto;
        }

        public PointOfInterestDto CreatePointOfInterestById(int cityId, PointOfInterestForCreationDto pointOfInterestDto)
        {
            var city = _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Result;

            if (city == null)
            {
                return null;
            }

            var results = new List<PointOfInterest>();

            var pointOfInterests = _cityInfoRepository.GetAllPointsOfInterestForCityAsync(cityId).Result;

            foreach (var pointInPointOfInterest in pointOfInterests)
            {
                //var point = _mapper.Map(pointInPointOfInterest);

                results.Add(pointInPointOfInterest);
            }

            var maxPointOfInterestId = results.Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterest()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestDto.Name,
                Description = pointOfInterestDto.Description
            };

            results.Add(finalPointOfInterest);

            return GetPointOfInterestById(cityId, finalPointOfInterest.Id);
        }

        public PointOfInterestDto UpdatePointOfInterestById(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestDto)
        {
            var city = _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Result;

            if (city == null)
            {
                return null;
            }

            var pointOfInterestFromStore = _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Result;

            if (pointOfInterestFromStore == null)
            {
                return null;
            }

            pointOfInterestFromStore.Name = pointOfInterestDto.Name;
            pointOfInterestFromStore.Description = pointOfInterestDto.Description;

            PointOfInterestMapper pointOfInterestMapper = new PointOfInterestMapper();

            var pointOfInterestFromStoreDto = pointOfInterestMapper.Map(pointOfInterestFromStore);

            return pointOfInterestFromStoreDto;
        }

        public PointOfInterestDto PartiallUpdatePointOfInterestById(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocumentDto)
        {
            var city = _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Result;

            if (city == null)
            {
                return null;
            }

            var pointOfInterestFromStore = _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Result;

            if (pointOfInterestFromStore == null)
            {
                throw new Exception($"Point of Interset with id: {pointOfInterestId} not found");
            }

            var pointOfInterestToPatchDto = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocumentDto.ApplyTo(pointOfInterestToPatchDto);

            pointOfInterestFromStore.Name = pointOfInterestToPatchDto.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatchDto.Description;

            PointOfInterestMapper pointOfInterestMapper = new PointOfInterestMapper();

            var pointOfInterestFromStoreDto = pointOfInterestMapper.Map(pointOfInterestFromStore);

            return pointOfInterestFromStoreDto;
        }

        public PointOfInterestDto DeletePointOfInterestById(int cityId, int pointOfInterestId)
        {
            var city = _cityInfoRepository.GetCityByCityIdAsync(cityId, true).Result;

            if (city == null)
            {
                return null;
            }

            var pointOfInterestFromStore = _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Result;

            if (pointOfInterestFromStore == null)
            {
                return null;
            }

            city.PointOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send("Point of interest deleted",
                $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

            PointOfInterestMapper pointOfInterestMapper = new PointOfInterestMapper();

            var pointOfInterestFromStoreDto = pointOfInterestMapper.Map(pointOfInterestFromStore);

            return pointOfInterestFromStoreDto;
        }
    }
}
