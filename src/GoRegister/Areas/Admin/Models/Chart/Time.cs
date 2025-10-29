using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Time
    {
        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}