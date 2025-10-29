using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Yax
    {
        [JsonProperty("ticks")]
        public Ticks Ticks { get; set; }

        [JsonProperty("gridLines")]
        public Gridlines GridLines { get; set; }
    }
}