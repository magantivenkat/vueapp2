using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Scales
    {
        [JsonProperty("xAxes")]
        public Xax[] XAxes { get; set; }

        [JsonProperty("yAxes")]
        public Yax[] YAxes { get; set; }
    }
}