using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Gridlines
    {
        [JsonProperty("display")]
        public bool Display { get; set; }

        [JsonProperty("drawBorder")]
        public bool DrawBorder { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("zeroLineColor")]
        public string ZeroLineColor { get; set; }

        [JsonProperty("borderDash")]
        public int[] BorderDash { get; set; }

        [JsonProperty("zeroLineBorderDash")]
        public int[] ZeroLineBorderDash { get; set; }
    }
}