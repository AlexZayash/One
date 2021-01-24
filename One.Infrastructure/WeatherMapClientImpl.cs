using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using One.Core.DTO;
using One.Core.Interfaces;
using One.Infrastructure.WebWeatherClasses;

namespace One.Infrastructure
{
    public class CurrentWeather
    {
        public Coord coord { get; set; }
        public IEnumerable<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }


    public class WeatherMapClientImpl : IWeatherMapClient
    {
        private readonly string _key;
        private readonly HttpClient _httpClient;

        public WeatherMapClientImpl(string key, HttpClient httpClient)
        {
            _key = key;
            _httpClient = httpClient;
        }

        public async Task<WeatherDto> GetWeather(decimal lat, decimal lon)
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={_key}";
            var result = await _httpClient.GetStringAsync(url);
            var currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(result);
            return new WeatherDto
            {
                FeelLike = currentWeather.main.feels_like,
                Temp = currentWeather.main.temp,
                Pressure = currentWeather.main.pressure,
                Humidity = currentWeather.main.humidity,
                Date = DateTime.Now
            };
        }
    }
}