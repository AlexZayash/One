using One.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using One.Core.Interfaces;

namespace One.Core

{
    public class WeatherService
    {
        private readonly IWeatherMapClient _weatherMapClient;
        private readonly IWeatherRepo _weatherRepo;

        public WeatherService(IWeatherMapClient weatherMapClient, IWeatherRepo weatherRepo)
        {
            _weatherMapClient = weatherMapClient;
            _weatherRepo = weatherRepo;
        }

        public async Task<WeatherDto> GetWeather(decimal lat, decimal lon)
        {
            var id = await _weatherRepo.IsId(lat, lon);
            if (id != null)
            {
                return await _weatherRepo.GetWeatherFromDB(id);
            }
            var currentWebWeather = await _weatherMapClient.GetWeather(lat, lon);
            var newId = _weatherRepo.InsertCoord(lat, lon).GetAwaiter().GetResult();
            await _weatherRepo.InsertWeather(newId, currentWebWeather);
            return currentWebWeather;
        }

        public async Task UpdateDB()
        {
            List<CoordDto> list = await _weatherRepo.GetAllCoord();
            foreach (CoordDto l in list)
            {
                var currentWebWeather = await _weatherMapClient.GetWeather(l.Lat, l.Lon);
                await _weatherRepo.InsertWeather(l.Id, currentWebWeather);
            }
        }
    }
}
