using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class ChartJs
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; }

    }
}
