using One.Core;
using One.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace One.DB
{
    public class WeatherRepo : IWeatherRepo
    {
        private readonly string _connectionString;
        private readonly ILogger<WeatherRepo> _logger;

        public WeatherRepo(string connectionString, ILogger<WeatherRepo> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<string> IsId(decimal lat, decimal lon)
        {
            var context = new DbContextOptions<OneContext>();
            using var db = new OneContext(_connectionString);
            var res = (await db.Coords.FirstOrDefaultAsync(elem => elem.Lat == lat && elem.Lon == lon))?.Id;
            if (res == null)
            {
                _logger.LogInformation("index not exist");
            }
            _logger.LogInformation($"index:{res}");
            return res;
        }

        public async Task<WeatherDto> GetWeatherFromDB(string id)
        {
            using var db = new OneContext(_connectionString);
            DateTime lastDate = db.Weathers.Where(elem => elem.Id == id).Max(elem => elem.Date);
            var key = await db.Weathers.FirstOrDefaultAsync(elem => elem.Date == lastDate && elem.Id == id);
            if (key == null)
                return null;
            var res = await db.Weathers.FindAsync(key.Key);
            return new WeatherDto
            {
                Date = res.Date,
                FeelLike = res.FeelLike,
                Humidity = res.Humidity,
                Pressure = res.Pressure,
                Temp = res.Temp
            };
        }

        public async Task<string> InsertCoord(decimal lat, decimal lon)
        {
            using (var db = new OneContext(_connectionString))
            {
                await db.AddAsync(new Coord
                {
                    Id = Guid.NewGuid().ToString(),
                    Lat = lat,
                    Lon = lon
                });
                await db.SaveChangesAsync();
                var res = (await db.Coords.FirstOrDefaultAsync(elem => elem.Lat == lat && elem.Lon == lon))?.Id;
                _logger.LogInformation($"coordinations is added :{lat} and {lon}");
                return res;
            }
        }

        public async Task InsertWeather(string id, WeatherDto currentWeather)
        {
            using (var db = new OneContext(_connectionString))
            {
                await db.AddAsync(new Weather
                {
                    Key = Guid.NewGuid().ToString(),
                    Date = currentWeather.Date,
                    FeelLike = currentWeather.FeelLike,
                    Id = id,
                    Humidity = currentWeather.Humidity,
                    Pressure = currentWeather.Pressure,
                    Temp = currentWeather.Temp
                });
                await db.SaveChangesAsync();
                _logger.LogInformation($"parameters of weather is added");
            }
        }


        public async Task<WeatherDto> GetCurrentWeather(decimal lat, decimal lon)
        {
            return await GetWeatherFromDB(await IsId(lat, lon));
        }

        public async Task<List<CoordDto>> GetAllCoord()
        {
            using var db = new OneContext(_connectionString);
            var list = await db.Coords.ToListAsync();
            List<CoordDto> res = new List<CoordDto>();
            foreach (var l in list)
            {
                res.Add(
                new CoordDto
                {
                    Id = l.Id,
                    Lat = l.Lat,
                    Lon = l.Lon
                });
            }
            return res;
        }

    }
}


