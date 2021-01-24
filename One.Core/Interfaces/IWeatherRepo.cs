using One.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace One.Core
{
    public interface IWeatherRepo
    {
        Task<WeatherDto> GetCurrentWeather(decimal lat, decimal lon);
        Task<string> IsId(decimal lat, decimal lon);
        Task<WeatherDto> GetWeatherFromDB(string id);
        Task<string> InsertCoord(decimal lat, decimal lon);
        Task InsertWeather(string id, WeatherDto currentWeather);
        Task<List<CoordDto>> GetAllCoord();
    }
}