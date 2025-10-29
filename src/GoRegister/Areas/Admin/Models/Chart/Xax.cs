using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Xax
    {
        [JsonProperty("time")]
        public Time Time { get; set; }

        [JsonProperty("gridLines")]
        public Gridlines GridLines { get; set; }

        [JsonProperty("ticks")]
        public Ticks Ticks { get; set; }
    }
}