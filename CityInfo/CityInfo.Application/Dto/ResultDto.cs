using System.Collections.Generic;

namespace CityInfo.Application.Dto
{
    public class ResultDto<T>
    {
        public IEnumerable<T> Results { get; set; }
    }
}
