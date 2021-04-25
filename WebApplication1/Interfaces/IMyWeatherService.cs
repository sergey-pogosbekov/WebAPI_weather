using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.App_Code;

namespace WebApplication1.Interfaces
{
    public interface IMyWeatherService
    {
        Task<IEnumerable<CityInfo>> GetCitiesByText(string text);
        Task<WeatherInfo> GetWeatherInfoByCityKey(string idKey);
    }
}
