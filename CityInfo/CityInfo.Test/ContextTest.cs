using AutoMapper;
using CityInfo.Application.Profiles;

namespace CityInfo.Test
{
    public class ContextTest
    {
        public static IMapper _mapper;

        public ContextTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new CityProfile());
                    mc.AddProfile(new PointOfInterestProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
    }
}
