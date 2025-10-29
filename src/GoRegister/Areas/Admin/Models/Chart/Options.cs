using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Models.Chart
{
    public class Options
    {
        [JsonProperty("maintainAspectRatio")]
        public bool MaintainAspectRatio { get; set; }

        [JsonProperty("layout")]
        public Layout Layout { get; set; }

        [JsonProperty("scales")]
        public Scales Scales { get; set; }

        [JsonProperty("legend")]
        public Legend Legend { get; set; }

        [JsonProperty("tooltips")]
        public Tooltips Tooltips { get; set; }
    }
}