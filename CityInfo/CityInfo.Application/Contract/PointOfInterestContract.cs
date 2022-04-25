using AutoMapper;
using CityInfo.Application.Dto;
using CityInfo.Application.Mapper;
using CityInfo.Domain;
using CityInfo.Infrastructure.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public class PointOfInterestContract : IPointOfInterestContract
    {
        private readonly ILogger<PointOfInterestContract> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointOfInterestContract(ILogger<PointOfInterestContract> logger,
                                       IMailService mailService,
                                       ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger;
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<PointOfInterestDto> GetAllPointsOfInterestByCityId(int cityId)
        {

            if (!_cityInfoRepository.CityForCityIdExists(cityId).Result)
            {
                _logger.LogCritical($"City with id {cityId} wasn't found when accessing points of interest.");
                return null;
            }

            var allPointOfInterests = _cityInfoRepository.GetAllPointsOfInterestForCityAsync(cityId).Result;

            if (allPointOfInterests == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<PointOfInterestDto>>(allPointOfInterests);
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

            return _mapper.Map<PointOfInterestDto>(pointOfInterest);
        }

        public async Task<PointOfInterestDto> CreatePointOfInterestById(int cityId, PointOfInterestForCreationDto pointOfInterestDto)
        {
            if (!_cityInfoRepository.CityForCityIdExists(cityId).Result)
            {
                _logger.LogCritical($"City with id {cityId} wasn't found when accessing points of interest.");
                return null;
            }

            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterestDto);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<Application.Dto.PointOfInterestDto>(finalPointOfInterest);

            return createdPointOfInterestToReturn;
        }

        public async Task<PointOfInterestDto> UpdatePointOfInterestById(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestDto)
        {
            if (!_cityInfoRepository.CityForCityIdExists(cityId).Result)
            {
                _logger.LogCritical($"City with id {cityId} wasn't found when accessing points of interest.");
                return null;
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Result;

            if (pointOfInterestEntity == null)
            {
                return null;
            }
            

            var pointOfInterestToReturn = _mapper.Map(pointOfInterestDto, pointOfInterestEntity);

            var updatePointOfIntrestStatus = _cityInfoRepository.UpdatePointOfInterestForCity(pointOfInterestToReturn);

            if (updatePointOfIntrestStatus)
            {
                await _cityInfoRepository.SaveChangesAsync();
            }

            var result = new PointOfInterestDto();

            return _mapper.Map(pointOfInterestToReturn, result);
        }

        public async Task<PointOfInterestDto> PartiallUpdatePointOfInterestById(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocumentDto)
        {
            if (!_cityInfoRepository.CityForCityIdExists(cityId).Result)
            {
                _logger.LogCritical($"City with id {cityId} wasn't found when accessing points of interest.");
                return null;
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Result;

            if (pointOfInterestEntity == null)
            {
                return null;
            }

            var pointOfInterestToPatchDto = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocumentDto.ApplyTo(pointOfInterestToPatchDto);

            var pointOfInterestToReturn = _mapper.Map(pointOfInterestToPatchDto, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            var result = new PointOfInterestDto();

            return _mapper.Map(pointOfInterestToReturn, result);
        }

        public async Task<PointOfInterestDto> DeletePointOfInterestById(int cityId, int pointOfInterestId)
        {
            if (!_cityInfoRepository.CityForCityIdExists(cityId).Result)
            {
                _logger.LogCritical($"City with id {cityId} wasn't found when accessing points of interest.");
                return null;
            }

            var pointOfInterestEntity = _cityInfoRepository
                .GetPointOfInterestForCityByPointOfInterestIdAsync(cityId, pointOfInterestId).Result;

            if (pointOfInterestEntity == null)
            {
                return null;
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Point of interest deleted",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            var result = new PointOfInterestDto();

            return _mapper.Map(pointOfInterestEntity, result);
        }
    }
}
