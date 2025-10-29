using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Layout
    {
        [JsonProperty("padding")]
        public Padding Padding { get; set; }
    }
}