using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using One.Core;
using One.Core.DTO;

namespace One.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet("{lat}/{lon}")]
        public async Task<WeatherDto> GetAsync(decimal lat, decimal lon)
        {
            var weather = await _weatherService.GetWeather(lat, lon);
            return weather;
        }
    }
}
