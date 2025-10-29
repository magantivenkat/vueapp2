using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Legend
    {
        [JsonProperty("display")]
        public bool Display { get; set; }
    }
}