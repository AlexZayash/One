using System;
using System.Collections.Generic;
using System.Text;

namespace One.Core.DTO
{
    public class WeatherDto
    {
        public DateTime Date { get; set; }
        public int Pressure { get; set; }
        public decimal FeelLike { get; set; }
        public int Humidity { get; set; }
        public decimal Temp { get; set; }
    }
}
