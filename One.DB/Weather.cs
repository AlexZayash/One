using System;
using System.Collections.Generic;

#nullable disable

namespace One.DB
{
    public partial class Weather
    {
        public string Key { get; set; }
        public string Id { get; set; }
        public decimal Temp { get; set; }
        public decimal FeelLike { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public DateTime Date { get; set; }
    }
}
