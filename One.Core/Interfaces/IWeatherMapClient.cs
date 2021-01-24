using System.Threading.Tasks;
using One.Core.DTO;

namespace One.Core.Interfaces
{
    public interface IWeatherMapClient
    {
        Task<WeatherDto> GetWeather(decimal lat, decimal lon); 
    }
}
