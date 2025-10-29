using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Ticks
    {
        [JsonProperty("maxTicksLimit")]
        public int MaxTicksLimit { get; set; }
    }
}