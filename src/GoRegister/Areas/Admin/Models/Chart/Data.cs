using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Data
    {
        [JsonProperty("labels")]
        public string[] Labels { get; set; }

        [JsonProperty("datasets")]
        public Dataset[] DataSets { get; set; }
    }
}